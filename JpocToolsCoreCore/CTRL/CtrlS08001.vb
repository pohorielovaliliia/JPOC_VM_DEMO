Imports System.IO
Public Class CtrlS08001
    Inherits AbstractCtrl

#Region "インスタンス変数"

#End Region

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS08001
        Get
            Return CType(MyBase.mDto, DtoS08001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS08001
        Get
            Return CType(MyBase.mLogic, LogicS08001)
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDto As AbstractDto)
        MyBase.New(pDto)
    End Sub
#End Region

#Region "初期化"
    Public Overrides Sub Init()
        Try

        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "Dispose"
    Public Shadows Sub Dispose()
        MyBase.Dispose()
    End Sub
#End Region

#Region "保存"
    Public Sub Save()
        Dim warningExist As Boolean = False
        Dim errorExist As Boolean = False
        Try
            '入力チェック
            Me.Logic.CheckEntry()
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub
            If Me.dto.NeedBase64Encode Then
                For Each tbl As DataTable In Me.dto.DiseaseDataSet.Tables
                    For Each dr As DataRow In tbl.Rows
                        For Each col As DataColumn In tbl.Columns
                            If col.DataType.Equals(Type.GetType("System.String")) Then
                                If Not dr.IsNull(col.ColumnName) Then
                                    Dim s As String = Utilities.NZ(dr.Item(col.ColumnName))
                                    s = Utilities.ConvertStringToBase64(s)
                                    dr.Item(col.ColumnName) = s
                                End If
                            End If
                        Next
                    Next
                Next
                Me.dto.DiseaseDataSet.AcceptChanges()
            End If
            Me.dto.DiseaseDataSet.WriteXml(Me.dto.OutputFilePath, XmlWriteMode.WriteSchema)
            Me.dto.RtnCD = PublicEnum.eRtnCD.Normal
            Me.dto.MessageSet = Utilities.GetMessageSet("INF0000", "保存しました。")
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "データ取得"
    Public Sub GetDiseaseData()
        Try
            '入力チェック
            Me.Logic.CheckEntry()
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            Me.Logic.GetDiseaseData()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

End Class
