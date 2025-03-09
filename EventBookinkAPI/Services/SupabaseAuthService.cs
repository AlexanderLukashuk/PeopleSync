using Supabase;
using Supabase.Gotrue;

namespace EventBookinkAPI.Services;

public class SupabaseAuthService
{
    private readonly Supabase.Client supabase;

    public SupabaseAuthService()
    {
        supabase = new Supabase.Client("https://fonvymwobgwlqmrcyuip.supabase.co",
                                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZvbnZ5bXdvYmd3bHFtcmN5dWlwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDE0Mjg2MjEsImV4cCI6MjA1NzAwNDYyMX0.f54MYzM34DGZR2z6xGWomOwv-jGMwzZSmn5kNDZSwUQ",
                                new SupabaseOptions { AutoConnectRealtime = false });
    }

    public async Task<Session> Register(string email, string password)
    {
        var response = await supabase.Auth.SignUp(email, password);
        return response;
    }

    public async Task<Session> Login(string email, string password)
    {
        var response = await supabase.Auth.SignIn(email, password);
        return response;
    }
}