namespace JobSearcher.CoreDomains.StorageDomains.Otp;

public interface IOtpService
{
    Task InsertOtpAsync(OTP otp);
    public string GenerateOtpCodeAsync(int lenght);
    Task<OTP?> GetOtpAsync(string? Code);
}