
Public Class OnStepEventArgs
    Inherits EventArgs
    Public Property StepEventType As PublicEnum.eStepEventType
    Public Property Target As String
    Public Property Message As String
    Public Property CompleteMainStep As Integer
    Public Property TotalMainStep As Integer
    Public Property CompleteSubStep As Integer
    Public Property TotalSubStep As Integer
    Public Property CompleteTransactionStep As Integer
    Public Property TotalTransactionStep As Integer

    Public Sub New()
        MyBase.New()
        Me._StepEventType = PublicEnum.eStepEventType.Message
        Me._Target = String.Empty
        Me._Message = String.Empty
        Me._CompleteMainStep = 0
        Me._TotalMainStep = 0
        Me._CompleteSubStep = 0
        Me._TotalSubStep = 0
        Me._CompleteTransactionStep = 0
        Me._TotalTransactionStep = 0
    End Sub
End Class