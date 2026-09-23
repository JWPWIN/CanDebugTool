using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// 诊断窗口本机配置：请求/响应 ID 与 DID 列表。
/// </summary>
public class UdsDiagConfig
{
    public string SchemaVersion { get; set; } = "1.0";
    public string ReqId { get; set; } = "0x7E0";
    public string RespId { get; set; } = "0x7E8";
    public int FrameTypeIndex { get; set; }
    public int PaddingIndex { get; set; }
    public int TimeoutMs { get; set; } = 2000;
    public int SessionIndex { get; set; } = 2;
    public bool TesterPresent { get; set; }
    public int TpPeriodMs { get; set; } = 2000;
    public List<UdsDidItem> Dids { get; set; } = new();
}

public class UdsDidItem
{
    public bool Enabled { get; set; } = true;
    public string Did { get; set; } = "";
    public string Name { get; set; } = "";
}

public static class UdsDiagConfigStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string FilePath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CanDebugTool", "uds-diag.json");

    public static List<UdsDidItem> DefaultDids()
    {
        return new List<UdsDidItem>
        {
            new() { Enabled = true, Did = "F187", Name = "供应商零件号" },
            new() { Enabled = true, Did = "F18A", Name = "系统供应商" },
            new() { Enabled = true, Did = "F190", Name = "VIN" },
            new() { Enabled = false, Did = "F191", Name = "车辆制造商硬件号" },
            new() { Enabled = false, Did = "F189", Name = "车辆制造商软件号" },
            new() { Enabled = false, Did = "F197", Name = "系统名称" }
        };
    }

    public static UdsDiagConfig LoadOrDefault()
    {
        try
        {
            string path = FilePath;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var cfg = JsonSerializer.Deserialize<UdsDiagConfig>(json, JsonOptions);
                if (cfg is not null)
                {
                    cfg.Dids ??= new List<UdsDidItem>();
                    if (cfg.Dids.Count == 0)
                        cfg.Dids = DefaultDids();
                    return cfg;
                }
            }
        }
        catch
        {
            // 损坏或权限问题：回退默认
        }

        return new UdsDiagConfig { Dids = DefaultDids() };
    }

    public static void Save(UdsDiagConfig config)
    {
        if (config is null)
            return;
        config.SchemaVersion = "1.0";
        string dir = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(config, JsonOptions));
    }

    public static bool TrySave(UdsDiagConfig config)
    {
        try
        {
            Save(config);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
