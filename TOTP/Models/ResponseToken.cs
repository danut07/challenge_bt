namespace TOTP.Models
{
    record ResponseToken(string Totp, int ExpiryTime);
}
