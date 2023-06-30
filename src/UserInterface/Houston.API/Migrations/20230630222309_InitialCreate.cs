using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Houston.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "houston");

            migrationBuilder.CreateTable(
                name: "TriggerEvent",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TriggerEvent_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TriggerFilter",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TriggerFilter_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    email = table.Column<string>(type: "character varying", nullable: false),
                    password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    first_access = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_User_id_created_by",
                        column: x => x.created_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_User_id_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Connector",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Connector_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_User_id_created_by",
                        column: x => x.created_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_User_id_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Pipeline",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pipeline_pk", x => x.id);
                    table.ForeignKey(
                        name: "User_id_created_by_fk",
                        column: x => x.created_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ConnectorFunction",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    connector_id = table.Column<Guid>(type: "uuid", nullable: false),
                    script = table.Column<string[]>(type: "character varying[]", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ConnectorFunction_pk", x => x.id);
                    table.ForeignKey(
                        name: "Connector_id_connector_id_fk",
                        column: x => x.connector_id,
                        principalSchema: "houston",
                        principalTable: "Connector",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_created_by_fk",
                        column: x => x.created_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PipelineTrigger",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pipeline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_git = table.Column<string>(type: "character varying", nullable: false),
                    private_key = table.Column<string>(type: "character varying", nullable: false),
                    public_key = table.Column<string>(type: "character varying", nullable: false),
                    key_revealed = table.Column<bool>(type: "boolean", nullable: false),
                    secret = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PipelineTrigger_pk", x => x.id);
                    table.ForeignKey(
                        name: "Pipeline_id_pipeline_id_fk",
                        column: x => x.pipeline_id,
                        principalSchema: "houston",
                        principalTable: "Pipeline",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_created_by_fk",
                        column: x => x.created_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ConnectorFunctionInput",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    connector_function_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    placeholder = table.Column<string>(type: "character varying", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    required = table.Column<bool>(type: "boolean", nullable: false),
                    replace = table.Column<string>(type: "character varying", nullable: false),
                    values = table.Column<string[]>(type: "character varying[]", nullable: true),
                    default_value = table.Column<string>(type: "character varying", nullable: true),
                    advanced_option = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ConnectorFunctionInput_pk", x => x.id);
                    table.ForeignKey(
                        name: "ConnectorFunction_id_connector_function_id_fk",
                        column: x => x.connector_function_id,
                        principalSchema: "houston",
                        principalTable: "ConnectorFunction",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_created_by_fk",
                        column: x => x.created_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PipelineInstruction",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pipeline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    connector_function_id = table.Column<Guid>(type: "uuid", nullable: false),
                    connection = table.Column<Guid>(type: "uuid", nullable: true),
                    connected_to_array_index = table.Column<int>(type: "integer", nullable: true),
                    script = table.Column<string[]>(type: "character varying[]", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PipelineInstruction_pk", x => x.id);
                    table.ForeignKey(
                        name: "PipelineInstruction_connector_function_id_fk",
                        column: x => x.connector_function_id,
                        principalSchema: "houston",
                        principalTable: "ConnectorFunction",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "PipelineInstruction_id_connection_fk",
                        column: x => x.connection,
                        principalSchema: "houston",
                        principalTable: "PipelineInstruction",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Pipeline_id_pipeline_id_fk",
                        column: x => x.pipeline_id,
                        principalSchema: "houston",
                        principalTable: "Pipeline",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_created_by_id",
                        column: x => x.created_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PipelineTriggerEvent",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pipeline_trigger_id = table.Column<Guid>(type: "uuid", nullable: false),
                    trigger_event_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PipelineTriggerEvent_pk", x => x.id);
                    table.ForeignKey(
                        name: "PipelineTriggerEvent_pipeline_trigger_id_fk",
                        column: x => x.pipeline_trigger_id,
                        principalSchema: "houston",
                        principalTable: "PipelineTrigger",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PipelineTriggerEvent_trigger_event_id_fk",
                        column: x => x.trigger_event_id,
                        principalSchema: "houston",
                        principalTable: "TriggerEvent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PipelineInstructionInput",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    input_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instruction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    replace_value = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PipelineInstructionInput_pk", x => x.id);
                    table.ForeignKey(
                        name: "ConnectorFunctionInput_id_input_id_fk",
                        column: x => x.input_id,
                        principalSchema: "houston",
                        principalTable: "ConnectorFunctionInput",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "PipelineInstruction_id_input_id_fk",
                        column: x => x.instruction_id,
                        principalSchema: "houston",
                        principalTable: "PipelineInstruction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "User_id_created_by_fk",
                        column: x => x.created_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PipelineLog",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pipeline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exit_code = table.Column<long>(type: "bigint", nullable: false),
                    run_n = table.Column<long>(type: "bigint", nullable: false),
                    stdout = table.Column<string>(type: "text", nullable: true),
                    instruction_with_error = table.Column<Guid>(type: "uuid", nullable: true),
                    triggered_by = table.Column<Guid>(type: "uuid", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp(3) with time zone", nullable: false),
                    duration = table.Column<TimeSpan>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PipelineLog_pk", x => x.id);
                    table.ForeignKey(
                        name: "PipelineInstruction_id_instruction_with_error",
                        column: x => x.instruction_with_error,
                        principalSchema: "houston",
                        principalTable: "PipelineInstruction",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Pipeline_id_pipeline_id_fk",
                        column: x => x.pipeline_id,
                        principalSchema: "houston",
                        principalTable: "Pipeline",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_triggered_by_fk",
                        column: x => x.triggered_by,
                        principalSchema: "houston",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PipelineTriggerFilter",
                schema: "houston",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pipeline_trigger_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    trigger_filter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    filter_values = table.Column<string[]>(type: "character varying[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PipelineTriggerFilter_pk", x => x.id);
                    table.ForeignKey(
                        name: "PipelineTriggerFilter_pipeline_trigger_event_id_fk",
                        column: x => x.pipeline_trigger_event_id,
                        principalSchema: "houston",
                        principalTable: "PipelineTriggerEvent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PipelineTriggerFilter_trigger_filter_id_fk",
                        column: x => x.trigger_filter_id,
                        principalSchema: "houston",
                        principalTable: "TriggerFilter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "houston",
                table: "TriggerEvent",
                columns: new[] { "id", "value" },
                values: new object[,]
                {
                    { new Guid("c0437ca0-a971-4d40-99f6-2a3c35e6fb41"), "push" },
                    { new Guid("e9b3eb7e-526b-4f89-968c-7cc0f60228cd"), "pull_request" }
                });

            migrationBuilder.InsertData(
                schema: "houston",
                table: "TriggerFilter",
                columns: new[] { "id", "value" },
                values: new object[,]
                {
                    { new Guid("24a42711-ed13-405b-8527-b5e53c680b4d"), "branches" },
                    { new Guid("aecde3fd-e2cf-4817-9701-178305697f46"), "tags" },
                    { new Guid("e859f16a-588b-46e2-b9f4-f7b60051e387"), "types" },
                    { new Guid("f7c800a4-1f05-478f-9a0b-46fed919eae2"), "paths" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connector_created_by",
                schema: "houston",
                table: "Connector",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Connector_updated_by",
                schema: "houston",
                table: "Connector",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunction_connector_id",
                schema: "houston",
                table: "ConnectorFunction",
                column: "connector_id");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunction_created_by",
                schema: "houston",
                table: "ConnectorFunction",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunction_updated_by",
                schema: "houston",
                table: "ConnectorFunction",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunctionInput_connector_function_id",
                schema: "houston",
                table: "ConnectorFunctionInput",
                column: "connector_function_id");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunctionInput_created_by",
                schema: "houston",
                table: "ConnectorFunctionInput",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunctionInput_updated_by",
                schema: "houston",
                table: "ConnectorFunctionInput",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Pipeline_created_by",
                schema: "houston",
                table: "Pipeline",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Pipeline_updated_by",
                schema: "houston",
                table: "Pipeline",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstruction_connection",
                schema: "houston",
                table: "PipelineInstruction",
                column: "connection");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstruction_connector_function_id",
                schema: "houston",
                table: "PipelineInstruction",
                column: "connector_function_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstruction_created_by",
                schema: "houston",
                table: "PipelineInstruction",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstruction_pipeline_id",
                schema: "houston",
                table: "PipelineInstruction",
                column: "pipeline_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstruction_updated_by",
                schema: "houston",
                table: "PipelineInstruction",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstructionInput_created_by",
                schema: "houston",
                table: "PipelineInstructionInput",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstructionInput_input_id",
                schema: "houston",
                table: "PipelineInstructionInput",
                column: "input_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstructionInput_instruction_id",
                schema: "houston",
                table: "PipelineInstructionInput",
                column: "instruction_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineInstructionInput_updated_by",
                schema: "houston",
                table: "PipelineInstructionInput",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineLog_instruction_with_error",
                schema: "houston",
                table: "PipelineLog",
                column: "instruction_with_error");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineLog_pipeline_id",
                schema: "houston",
                table: "PipelineLog",
                column: "pipeline_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineLog_triggered_by",
                schema: "houston",
                table: "PipelineLog",
                column: "triggered_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTrigger_created_by",
                schema: "houston",
                table: "PipelineTrigger",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTrigger_pipeline_id",
                schema: "houston",
                table: "PipelineTrigger",
                column: "pipeline_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTrigger_updated_by",
                schema: "houston",
                table: "PipelineTrigger",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTriggerEvent_pipeline_trigger_id",
                schema: "houston",
                table: "PipelineTriggerEvent",
                column: "pipeline_trigger_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTriggerEvent_trigger_event_id",
                schema: "houston",
                table: "PipelineTriggerEvent",
                column: "trigger_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTriggerFilter_pipeline_trigger_event_id",
                schema: "houston",
                table: "PipelineTriggerFilter",
                column: "pipeline_trigger_event_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTriggerFilter_trigger_filter_id",
                schema: "houston",
                table: "PipelineTriggerFilter",
                column: "trigger_filter_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_created_by",
                schema: "houston",
                table: "User",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_User_updated_by",
                schema: "houston",
                table: "User",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "User_email_uq",
                schema: "houston",
                table: "User",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PipelineInstructionInput",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "PipelineLog",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "PipelineTriggerFilter",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "ConnectorFunctionInput",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "PipelineInstruction",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "PipelineTriggerEvent",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "TriggerFilter",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "ConnectorFunction",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "PipelineTrigger",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "TriggerEvent",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "Connector",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "Pipeline",
                schema: "houston");

            migrationBuilder.DropTable(
                name: "User",
                schema: "houston");
        }
    }
}
