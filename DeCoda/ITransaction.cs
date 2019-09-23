using System;
namespace DeCoda
{
    public interface ITransaction
    {
        string StructuredMessage { get; set; }
        DateTime TransactionDate { get; set; }
        DateTime ValueDate { get; set; }
        decimal Amount { get; set; }
        string Message { get; set; }
        string Adresse { get; set; }
        string Iban { get; set; }
        string Name { get; set; }
    }
}
