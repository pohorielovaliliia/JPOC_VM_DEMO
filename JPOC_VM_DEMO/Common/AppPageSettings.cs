using System;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace JPOC_VM_DEMO.Common
{
    /// <summary>
    /// PageSettings.xmlから設定情報を取得する
    /// </summary>
    public class AppPageSettings
    {
        #region Constants
        /// <summary>
        /// キー値を保持する列名
        /// </summary>
        private const string NAME_COL = "NAME";

        /// <summary>
        /// 値を保持する列名
        /// </summary>
        private const string VAL_COL = "VALUE";
        #endregion

        #region Variables
        /// <summary>
        /// Singletonインスタンス
        /// </summary>
        private static AppPageSettings _appPageSettings;

        /// <summary>
        /// 設定情報を格納するデータセット
        /// </summary>
        private DataSet _ConfigDataSet;

        /// <summary>
        /// 設定ファイルのフルパス
        /// </summary>
        private string _ConfigFileFullPath;

        /// <summary>
        /// ファイル情報(更新確認用)
        /// </summary>
        private FileInfo _fileInfo;
        #endregion

        #region Constructors
        protected AppPageSettings()
        {
            _ConfigDataSet = new DataSet();
        }

        protected AppPageSettings(string configFilePath) : this()
        {
            _ConfigFileFullPath = configFilePath;
            Init();
        }
        #endregion

        #region Initialization
        /// <summary>
        /// 構成ファイルから設定値を読み取りデータセットに格納する。
        /// </summary>
        private void Init()
        {
            _fileInfo = new FileInfo(_ConfigFileFullPath);
            var ds = new DataSet();
            Utilities.XmlToDataSet(ref ds, _ConfigFileFullPath);
            _ConfigDataSet = new DataSet();

            foreach (DataTable tbl in ds.Tables)
            {
                _ConfigDataSet.Tables.Add(tbl.Copy());
            }

            ds.Dispose();

#if DEBUG
            Utilities.DataDump(_ConfigDataSet);
#endif

            foreach (DataTable tbl in _ConfigDataSet.Tables)
            {
                var pk = new[] { tbl.Columns[NAME_COL] };
                tbl.PrimaryKey = pk;
            }

            _ConfigDataSet.AcceptChanges();
        }
        #endregion

        #region Singleton Instance
        /// <summary>
        /// シングルトンインスタンス取得
        /// </summary>
        public static AppPageSettings GetInstance(string configFileFullPath)
        {
            return _appPageSettings ??= new AppPageSettings(configFileFullPath);
        }
        #endregion

        #region Configuration Access
        /// <summary>
        /// 設定値取得
        /// </summary>
        /// <param name="pageName">画面名</param>
        /// <param name="configName">キー</param>
        /// <param name="whenNoEntryRaiseException">キーが見つからなかった場合に例外を発生させる</param>
        public string GetConfig(string pageName, string configName, bool whenNoEntryRaiseException = true)
        {
            CheckUpdate();

            var dr = _ConfigDataSet.Tables[pageName]?.Rows.Find(configName);

            if (dr == null)
            {
                if (whenNoEntryRaiseException)
                {
                    throw new ArgumentNullException($"設定ファイルに{pageName}.{configName}が設定されていません。");
                }
                return string.Empty;
            }

            return Utilities.NZ(dr[VAL_COL]);
        }
        #endregion

        #region Configuration File Update Check
        private void CheckUpdate()
        {
            var fi = new FileInfo(_ConfigFileFullPath);
            if (fi.LastWriteTime != _fileInfo.LastWriteTime)
            {
                Init();
            }
        }
        #endregion
    }
}
