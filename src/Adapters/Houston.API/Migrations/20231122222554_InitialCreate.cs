using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Houston.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "houston_v2");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "houston_v2",
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
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_User_id_created_by",
                        column: x => x.created_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_User_id_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Connector",
                schema: "houston_v2",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    friendly_name = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Connector_pk", x => x.id);
                    table.ForeignKey(
                        name: "fk_User_id_created_by",
                        column: x => x.created_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_User_id_updated_by",
                        column: x => x.updated_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Pipeline",
                schema: "houston_v2",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    spec_file = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pipeline_pk", x => x.id);
                    table.ForeignKey(
                        name: "User_id_created_by_fk",
                        column: x => x.created_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ConnectorFunction",
                schema: "houston_v2",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    friendly_name = table.Column<string>(type: "character varying", nullable: false),
                    version = table.Column<string>(type: "character varying", nullable: false),
                    specs_file = table.Column<byte[]>(type: "bytea", nullable: false),
                    script = table.Column<byte[]>(type: "bytea", nullable: false),
                    package = table.Column<byte[]>(type: "bytea", nullable: false),
                    build_status = table.Column<int>(type: "integer", nullable: false),
                    build_stderr = table.Column<string>(type: "character varying", nullable: true),
                    script_dist = table.Column<byte[]>(type: "bytea", nullable: true),
                    package_type = table.Column<int>(type: "integer", nullable: true),
                    connector_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ConnectorFunction_pk", x => x.id);
                    table.ForeignKey(
                        name: "Connector_id_connector_id_fk",
                        column: x => x.connector_id,
                        principalSchema: "houston_v2",
                        principalTable: "Connector",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_created_by_fk",
                        column: x => x.created_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PipelineLog",
                schema: "houston_v2",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pipeline_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exit_code = table.Column<long>(type: "bigint", nullable: false),
                    stdout = table.Column<string>(type: "text", nullable: true),
                    triggered_by = table.Column<Guid>(type: "uuid", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    duration = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    spec_file = table.Column<byte[]>(type: "bytea", nullable: false),
                    step_error = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PipelineLog_pk", x => x.id);
                    table.ForeignKey(
                        name: "Pipeline_id_pipeline_id_fk",
                        column: x => x.pipeline_id,
                        principalSchema: "houston_v2",
                        principalTable: "Pipeline",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_triggered_by_fk",
                        column: x => x.triggered_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PipelineTrigger",
                schema: "houston_v2",
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
                    creation_date = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp(3) with time zone", precision: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PipelineTrigger_pk", x => x.id);
                    table.ForeignKey(
                        name: "Pipeline_id_pipeline_id_fk",
                        column: x => x.pipeline_id,
                        principalSchema: "houston_v2",
                        principalTable: "Pipeline",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_created_by_fk",
                        column: x => x.created_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "User_id_updated_by_fk",
                        column: x => x.updated_by,
                        principalSchema: "houston_v2",
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "Connector_ukey",
                schema: "houston_v2",
                table: "Connector",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connector_created_by",
                schema: "houston_v2",
                table: "Connector",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Connector_updated_by",
                schema: "houston_v2",
                table: "Connector",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ConnectorFunction_ukey",
                schema: "houston_v2",
                table: "ConnectorFunction",
                columns: new[] { "name", "connector_id", "version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunction_connector_id",
                schema: "houston_v2",
                table: "ConnectorFunction",
                column: "connector_id");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunction_created_by",
                schema: "houston_v2",
                table: "ConnectorFunction",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectorFunction_updated_by",
                schema: "houston_v2",
                table: "ConnectorFunction",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Pipeline_created_by",
                schema: "houston_v2",
                table: "Pipeline",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_Pipeline_updated_by",
                schema: "houston_v2",
                table: "Pipeline",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineLog_pipeline_id",
                schema: "houston_v2",
                table: "PipelineLog",
                column: "pipeline_id");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineLog_triggered_by",
                schema: "houston_v2",
                table: "PipelineLog",
                column: "triggered_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTrigger_created_by",
                schema: "houston_v2",
                table: "PipelineTrigger",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTrigger_pipeline_id",
                schema: "houston_v2",
                table: "PipelineTrigger",
                column: "pipeline_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PipelineTrigger_updated_by",
                schema: "houston_v2",
                table: "PipelineTrigger",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "IX_User_created_by",
                schema: "houston_v2",
                table: "User",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_User_updated_by",
                schema: "houston_v2",
                table: "User",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "User_email_uq",
                schema: "houston_v2",
                table: "User",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectorFunction",
                schema: "houston_v2");

            migrationBuilder.DropTable(
                name: "PipelineLog",
                schema: "houston_v2");

            migrationBuilder.DropTable(
                name: "PipelineTrigger",
                schema: "houston_v2");

            migrationBuilder.DropTable(
                name: "Connector",
                schema: "houston_v2");

            migrationBuilder.DropTable(
                name: "Pipeline",
                schema: "houston_v2");

            migrationBuilder.DropTable(
                name: "User",
                schema: "houston_v2");
        }
    }
}
