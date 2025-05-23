Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlDbType
Public MustInherit Class AbstractSoredProcedureDao
    Inherits AbstractDao

#Region "定数"

#End Region

#Region "インスタンス変数"

#End Region

#Region "プロパティ"

#End Region

#Region "コンストラクタ"
    Protected Sub New(ByRef pDbmanager As ElsDataBase)
        MyBase.New(pDbmanager)
    End Sub
#End Region

#Region "初期化"
    Public MustOverride Overrides Sub Init()
#End Region

#Region "リソース開放"
    Public Overrides Sub Dispose()
        MyBase.Dispose()
    End Sub
#End Region


End Class
