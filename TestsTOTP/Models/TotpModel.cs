
namespace TestsTOTP.Models
{
    public record TotpModel(string Username, int Timestamp, string Totp, int ExpectedExpiryTime);
}
