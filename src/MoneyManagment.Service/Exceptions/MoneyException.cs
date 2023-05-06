namespace MoneyManagment.Service.Exceptions;

public class MoneyException : Exception
{
    public int Code { get; set; }
    public MoneyException(int code, string message)
        : base(message)
    {
        this.Code = code;
    }
}
