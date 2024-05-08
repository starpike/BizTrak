namespace FinanceApp.Domain;

public class Invoice {
    public int Id { get; set;}
    public List<Job>? Jobs { get; set; }
}