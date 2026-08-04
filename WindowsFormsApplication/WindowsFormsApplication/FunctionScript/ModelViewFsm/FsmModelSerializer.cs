using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace WindowsFormsApplication.ModelViewFsm
{
    public static class FsmModelSerializer
    {
        public const string FileExtension = ".canfsm.json";
        public const string FileFilter = "CAN FSM Model (*.canfsm.json)|*.canfsm.json|JSON (*.json)|*.json";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static FsmModel Clone(FsmModel model)
        {
            string json = JsonSerializer.Serialize(model ?? new FsmModel(), JsonOptions);
            return JsonSerializer.Deserialize<FsmModel>(json, JsonOptions) ?? new FsmModel();
        }

        public static void Save(string path, FsmModel model)
        {
            model.SchemaVersion = "1.0";
            string json = JsonSerializer.Serialize(model, JsonOptions);
            File.WriteAllText(path, json);
        }

        public static FsmModel Load(string path)
        {
            string json = File.ReadAllText(path);
            var model = JsonSerializer.Deserialize<FsmModel>(json, JsonOptions)
                ?? throw new InvalidOperationException("配置文件为空或格式无效。");
            if (string.IsNullOrEmpty(model.SchemaVersion))
                model.SchemaVersion = "1.0";
            return model;
        }

        public static bool TryPickOpenPath(out string? path)
        {
            path = null;
            using var dlg = new OpenFileDialog
            {
                Filter = FileFilter,
                Multiselect = false
            };
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;
            path = dlg.FileName;
            return true;
        }

        public static bool TryPickSavePath(string suggestedName, out string? path)
        {
            path = null;
            using var dlg = new SaveFileDialog
            {
                Filter = FileFilter,
                FileName = suggestedName.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase)
                    ? suggestedName
                    : suggestedName + FileExtension
            };
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;
            path = dlg.FileName;
            if (!path.EndsWith(FileExtension, StringComparison.OrdinalIgnoreCase)
                && !path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                path += FileExtension;
            return true;
        }
    }
}
