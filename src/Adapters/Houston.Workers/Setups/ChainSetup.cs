using System.Linq.Expressions;

namespace Houston.Workers.Setups {
	public static class ChainSetup {
		public static IChainConfigurator<T> Chain<T>(this IServiceCollection services) where T : class {
			return new ChainConfiguratorImpl<T>(services);
		}

		public interface IChainConfigurator<T> {
			IChainConfigurator<T> Add<TImplementation>() where TImplementation : T;
			void Configure();
		}

		private sealed class ChainConfiguratorImpl<T> : IChainConfigurator<T> where T : class {
			private readonly IServiceCollection _services;
			private readonly List<Type> _types;
			private readonly Type _interfaceType;

			public ChainConfiguratorImpl(IServiceCollection services) {
				_services = services;
				_types = new List<Type>();
				_interfaceType = typeof(T);
			}

			public IChainConfigurator<T> Add<TImplementation>() where TImplementation : T {
				var type = typeof(TImplementation);

				_types.Add(type);

				return this;
			}

			public void Configure() {
				if (_types.Count == 0)
					throw new InvalidOperationException($"No implementation defined for {_interfaceType.Name}");

				foreach (var type in _types) {
					ConfigureType(type);
				}
			}

			private void ConfigureType(Type currentType) {
				var nextType = _types.SkipWhile(x => x != currentType).SkipWhile(x => x == currentType).FirstOrDefault();

				var parameter = Expression.Parameter(typeof(IServiceProvider), "x");

				var ctor = currentType.GetConstructors().OrderByDescending(x => x.GetParameters().Length).First();

				var ctorParameters = ctor.GetParameters().Select(p => {
					if (_interfaceType.IsAssignableFrom(p.ParameterType)) {
						if (nextType is null) {
							return Expression.Constant(null, _interfaceType);
						} else {
							return Expression.Call(typeof(ServiceProviderServiceExtensions), "GetRequiredService", new Type[] { nextType }, parameter);
						}
					}

					return (Expression)Expression.Call(typeof(ServiceProviderServiceExtensions), "GetRequiredService", new Type[] { p.ParameterType }, parameter);
				});

				var body = Expression.New(ctor, ctorParameters.ToArray());

				var first = _types[0] == currentType;
				var resolveType = first ? _interfaceType : currentType;
				var expressionType = Expression.GetFuncType(typeof(IServiceProvider), resolveType);
				var expression = Expression.Lambda(expressionType, body, parameter);
				var compiledExpression = (Func<IServiceProvider, object>)expression.Compile();

				_services.AddTransient(resolveType, compiledExpression);
			}
		}
	}
}
