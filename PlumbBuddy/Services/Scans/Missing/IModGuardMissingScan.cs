namespace PlumbBuddy.Services.Scans.Missing;

[Scan(IsEnabledByDefault = true)]
public interface IModGuardMissingScan :
    IMissingScan
{
}