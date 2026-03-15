using Microsoft.Extensions.Logging;
using Exception = System.Exception;
using File = System.IO.File;

namespace BetaSharp.Server;

internal class DedicatedServerConfiguration : IServerConfiguration
{
    public static ILogger<DedicatedServerConfiguration> logger = Log.Instance.For<DedicatedServerConfiguration>();
    private readonly ServerPropertiesFile properties = new();
    private readonly string propertiesFileName;

    public DedicatedServerConfiguration(string fileName)
    {
        propertiesFileName = fileName;

        if (File.Exists(propertiesFileName))
        {
            try
            {
                properties = ServerPropertiesFile.Load(propertiesFileName);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to load " + propertiesFileName);
                GenerateNew();
            }
        }
        else
        {
            logger.LogWarning(propertiesFileName + " does not exist");
            GenerateNew();
        }
    }

    private void GenerateNew()
    {
        logger.LogInformation("Generating new properties file");
        Save();
    }

    public void Save()
    {
        try
        {
            properties.Save(propertiesFileName);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to save " + propertiesFileName);
            GenerateNew();
        }
    }


    public string GetPropertyString(string property, string fallback)
    {
        if (!properties.ContainsKey(property))
        {
            properties.SetString(property, fallback);
            Save();
        }

        return properties.GetString(property, fallback);
    }

    public int GetPropertyInt(string property, int fallback)
    {
        try
        {
            return int.Parse(GetPropertyString(property, "" + fallback));
        }
        catch (Exception)
        {
            properties.SetInt(property, fallback);
            return fallback;
        }
    }

    public bool GetPropertyBool(string property, bool fallback)
    {
        try
        {
            return bool.Parse(GetPropertyString(property, "" + fallback));
        }
        catch (Exception)
        {
            properties.SetBool(property, fallback);
            return fallback;
        }
    }

    public void SetProperty(string property, bool value)
    {
        properties.SetBool(property, value);
        Save();
    }

    public string GetServerIp(string fallback) => GetPropertyString("server-ip", fallback);
    public int GetServerPort(int fallback) => GetPropertyInt("server-port", fallback);
    public bool GetDualStack(bool fallback) => GetPropertyBool("dual-stack", fallback);
    public bool GetOnlineMode(bool fallback) => GetPropertyBool("online-mode", fallback);
    public bool GetSpawnAnimals(bool fallback) => GetPropertyBool("spawn-animals", fallback);
    public bool GetPvpEnabled(bool fallback) => GetPropertyBool("pvp", fallback);
    public bool GetAllowFlight(bool fallback) => GetPropertyBool("allow-flight", fallback);
    public string GetLevelName(string fallback) => GetPropertyString("level-name", fallback);
    public string GetLevelType(string fallback) => GetPropertyString("level-type", fallback);
    public string GetLevelSeed(string fallback) => GetPropertyString("level-seed", fallback);
    public string GetLevelOptions(string fallback) => GetPropertyString("generator-settings", fallback);
    public bool GetSpawnMonsters(bool fallback) => GetPropertyBool("spawn-monsters", fallback);
    public bool GetAllowNether(bool fallback) => GetPropertyBool("allow-nether", fallback);
    public int GetMaxPlayers(int fallback) => GetPropertyInt("max-players", fallback);
    public int GetViewDistance(int fallback) => GetPropertyInt("view-distance", fallback);
    public bool GetWhiteList(bool fallback) => GetPropertyBool("white-list", fallback);
    public int GetSpawnRegionSize(int fallback) => GetPropertyInt("spawn-region-size", fallback);
}
