Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlDbType
Public MustInherit Class AbstractTableDao
    Inherits AbstractDao

#Region "定数"
    Protected Const SCOPE_IDENTITY As String = "SELECT SCOPE_IDENTITY();"
    Protected Const IDENTITY_INSERT_ON As String = "SET IDENTITY_INSERT {0} ON;"
    Protected Const IDENTITY_INSERT_OFF As String = "SET IDENTITY_INSERT {0} OFF;"
    Private Const ACTIVE_CONDITION As String = " {0}status='A' AND {0}defunct=0 "
    Private Const INACTIVE_CONDITION As String = " {0}status='I' AND {0}defunct=0 "
    Private Const ALL_CONDITION As String = " {0}defunct=0 "

#End Region

#Region "インスタンス変数"

#End Region

#Region "プロパティ"
    Protected ReadOnly Property ActiveCondition(Optional pTablePrefix As String = "") As String
        Get
            If String.IsNullOrEmpty(pTablePrefix) Then
                Return String.Format(ACTIVE_CONDITION, String.Empty)
            Else
                If pTablePrefix.Contains(".") Then
                    Return String.Format(ACTIVE_CONDITION, pTablePrefix)
                Else
                    Return String.Format(ACTIVE_CONDITION, pTablePrefix & ".")
                End If
            End If
        End Get
    End Property

    ''20170608
    Protected ReadOnly Property InActiveCondition(Optional pTablePrefix As String = "") As String
        Get
            If String.IsNullOrEmpty(pTablePrefix) Then
                Return String.Format(INACTIVE_CONDITION, String.Empty)
            Else
                If pTablePrefix.Contains(".") Then
                    Return String.Format(INACTIVE_CONDITION, pTablePrefix)
                Else
                    Return String.Format(INACTIVE_CONDITION, pTablePrefix & ".")
                End If
            End If
        End Get
    End Property

    Protected ReadOnly Property AllCondition(Optional pTablePrefix As String = "") As String
        Get
            If String.IsNullOrEmpty(pTablePrefix) Then
                Return String.Format(ALL_CONDITION, String.Empty)
            Else
                If pTablePrefix.Contains(".") Then
                    Return String.Format(ALL_CONDITION, pTablePrefix)
                Else
                    Return String.Format(ALL_CONDITION, pTablePrefix & ".")
                End If
            End If
        End Get
    End Property
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

#Region "件数取得"
    Public MustOverride Function GetCount() As Integer
#End Region

#Region "件数取得"
    Public MustOverride Function GetAll(ByRef pDt As DataTable, ByVal pOnlyActive As Boolean) As Integer
#End Region

End Class
