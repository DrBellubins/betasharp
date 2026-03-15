namespace BetaSharp.Server;

public interface IServerConfiguration
{
    string GetServerIp(string fallback);
    int GetServerPort(int fallback);
    bool GetDualStack(bool fallback);
    bool GetOnlineMode(bool fallback);
    bool GetSpawnAnimals(bool fallback);
    bool GetPvpEnabled(bool fallback);
    bool GetAllowFlight(bool fallback);
    string GetLevelName(string fallback);
    string GetLevelType(string fallback);
    string GetLevelSeed(string fallback);
    string GetLevelOptions(string fallback);
    bool GetSpawnMonsters(bool fallback);
    bool GetAllowNether(bool fallback);
    int GetMaxPlayers(int fallback);
    int GetViewDistance(int fallback);
    bool GetWhiteList(bool fallback);
    int GetSpawnRegionSize(int fallback);
    void Save();

    bool GetPropertyBool(string property, bool fallback);
    int GetPropertyInt(string property, int fallback);
    string GetPropertyString(string property, string fallback);
    void SetProperty(string property, bool value);
}
