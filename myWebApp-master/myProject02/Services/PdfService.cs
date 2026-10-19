namespace myProject02.Services
{
    using Microsoft.EntityFrameworkCore;
    using myProject02.Models;
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;

    public class PdfService : IPdfService
    {
        private readonly AppDbContext _context;

        public PdfService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerateVotersPdfAsync()
        {
            var voters = await _context.Voters
                .AsNoTracking()
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header()
                        .Text("Voter List")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .PaddingTop(20)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell()
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5)
                                    .Text("ID")
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5)
                                    .Text("Full Name")
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5)
                                    .Text("Email")
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5)
                                    .Text("District")
                                    .Bold();
                            });

                            // Data
                            foreach (var voter in voters)
                            {
                                table.Cell()
                                    .Padding(5)
                                    .Text(voter.Id.ToString());

                                table.Cell()
                                    .Padding(5)
                                    .Text(voter.FullName ?? "");

                                table.Cell()
                                    .Padding(5)
                                    .Text(voter.Email ?? "");

                                table.Cell()
                                    .Padding(5)
                                    .Text(voter.District ?? "");
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }


        public async Task<byte[]> GenerateUsersPdfAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header()
                        .Text("System User List")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .PaddingTop(20)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell()
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5)
                                    .Text("ID")
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5)
                                    .Text("Name")
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Grey.Lighten2)
                                    .Padding(5)
                                    .Text("Role")
                                    .Bold();
                            });

                            // Data
                            foreach (var user in users)
                            {
                                table.Cell()
                                    .Padding(5)
                                    .Text(user.Id.ToString());

                                table.Cell()
                                    .Padding(5)
                                    .Text(user.UserName ?? "");

                                table.Cell()
                                    .Padding(5)
                                    .Text(user.Role?.Name ?? "No Role");
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }

}
