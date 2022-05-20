using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains.Otp;
using Microsoft.EntityFrameworkCore;

namespace JobSearcher.CoreApplication.OtpApplication;

public class OtpService:IOtpService
{
    public DbSet<OTP> Otps;
    public OtpService(IUnitOfWork work)
    {
        Otps = work.Set<OTP>();
    }

    public async Task InsertOtpAsync(OTP otp) => await Otps.AddAsync(otp);

    public string GenerateOtpCodeAsync(int lenght)
    {
        var code = "";
        var randome = new Random();
        for (int i = 1; i <= lenght; i++)
        {
            code += randome.Next(1, 11);
        }
        return code;
    }

    public async Task<OTP?> GetOtpAsync(string Code)
    {
        return await Otps.FirstOrDefaultAsync(o => o.code == Code);
    }
}