namespace FinanceApp.Application.Services;

using FinanceApp.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class QuotePdfGenerator
{
    public static byte[] GenerateQuotePdf(Quote quote)
    {
        using var memoryStream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Content().Column(column =>
                {
                    column.Item()
                    .Background("#33ccff")
                    .Padding(15)
                    .AlignCenter()
                    .Text("Dulwich Handyman")
                    .FontSize(35)
                    .FontColor(Colors.White)
                    .Bold();

                    column.Item().PaddingVertical(5).Text("");

                    // Quote Reference
                    column.Item().Text($"Quote Ref: {quote.QuoteRef}").FontSize(14).Bold();
                    column.Item().PaddingBottom(20).Text($"Quote for: {quote.Title}").FontSize(14);

                    column.Item().Text("Stuart Kinlochan").FontSize(14);
                    column.Item().Text("07958 750605").FontSize(14);
                    column.Item().Text("dulwich.handyman@outlook.com").FontSize(14);

                    // Quote Details
                    column.Item().PaddingVertical(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("FOR:").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                            header.Cell().Text("DATE:").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                        });

                        table.Cell().Text(quote.Customer?.Name).FontSize(14);
                        table.Cell().Text(quote.CreatedAt.ToString("d MMM yyyy")).FontSize(14);
                        table.Cell().ColumnSpan(2).Text(quote.Customer?.Address).FontSize(14);
                    });

                    if (quote.Tasks.Count > 0)
                    {
                        // Tasks Banner
                        column.Item().PaddingBottom(5).Background("#33ccff").Padding(8).Text("Tasks:").FontSize(14).FontColor(Colors.White).Bold();

                        // Tasks Table
                        column.Item().PaddingBottom(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().PaddingVertical(5).PaddingHorizontal(8).Text("DESC").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                                header.Cell().PaddingVertical(5).PaddingHorizontal(8).AlignRight().Text("COST").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                            });

                            bool isGreyBackground = false;

                            foreach (var task in quote.Tasks)
                            {
                                var backgroundColor = isGreyBackground ? Colors.Grey.Lighten3 : Colors.White;
                                isGreyBackground = !isGreyBackground;

                                table.Cell().Background(backgroundColor).PaddingVertical(5).PaddingHorizontal(8).Text(task.Description).FontSize(14);
                                table.Cell().Background(backgroundColor).PaddingVertical(5).PaddingHorizontal(8).AlignRight().Text($"\u00a3{task.Cost:0.00}").FontSize(14);
                            }
                        });
                    }

                    // Materials Banner
                    if (quote.Materials.Count > 0)
                    {
                        column.Item().PaddingBottom(5).Background("#33ccff").Padding(8).Text("Materials:").FontSize(14).FontColor(Colors.White).Bold();

                        // Materials Table
                        column.Item().PaddingBottom(20).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().PaddingVertical(5).PaddingHorizontal(8).Text("ITEM").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                                header.Cell().PaddingVertical(5).PaddingHorizontal(8).Text("QTY").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                                header.Cell().PaddingVertical(5).PaddingHorizontal(8).Text("UNIT PRICE").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                                header.Cell().PaddingVertical(5).PaddingHorizontal(8).AlignRight().Text("COST").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                            });

                            bool isGreyBackground = false;

                            foreach (var material in quote.Materials)
                            {
                                var backgroundColor = isGreyBackground ? Colors.Grey.Lighten3 : Colors.White;
                                isGreyBackground = !isGreyBackground;

                                table.Cell().Background(backgroundColor).PaddingVertical(5).PaddingHorizontal(8).Text(material.MaterialName).FontSize(14);
                                table.Cell().Background(backgroundColor).PaddingVertical(5).PaddingHorizontal(8).Text(material.Quantity.ToString()).FontSize(14);
                                table.Cell().Background(backgroundColor).PaddingVertical(5).PaddingHorizontal(8).Text($"\u00a3{material.UnitPrice:0.00}").FontSize(14);
                                table.Cell().Background(backgroundColor).PaddingVertical(5).PaddingHorizontal(8).AlignRight().Text($"\u00a3{material.Quantity * material.UnitPrice:0.00}").FontSize(14);
                            }
                        });
                    }

                    // Quote Total
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().Text("Quote Total:").FontSize(14).Bold();
                        table.Cell().ColumnSpan(3).PaddingRight(8).AlignRight().Text($"\u00a3{quote.TotalAmount:0.00}").FontSize(14).Bold();
                    });

                    // column.Item().PaddingTop(30).Text(text =>
                    // {
                    //     text.Span("SORTCODE: ").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                    //     text.Span("601328").FontSize(14);
                    //     text.Span("                                             ").FontColor(Colors.Transparent); // Add invisible non-breaking space
                    //     text.Span("ACCOUNT NUMBER: ").FontSize(14).Bold().FontColor(Color.FromHex("4f4f4f"));
                    //     text.Span("78198674").FontSize(14);
                    // });

                    // Footer
                    column.Item().PaddingTop(40).Text("Kind regards,").FontSize(14);
                    column.Item().PaddingTop(20).Text("Stuart Kinlochan").FontSize(14);
                });
            });
        }).GeneratePdf(memoryStream);

        return memoryStream.ToArray();
    }
}

