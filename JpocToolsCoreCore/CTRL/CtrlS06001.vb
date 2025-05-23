Public Class CtrlS06001
    Inherits AbstractCtrl

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS06001
        Get
            Return CType(MyBase.mDto, DtoS06001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS06001
        Get
            Return CType(MyBase.mLogic, LogicS06001)
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


End Class
