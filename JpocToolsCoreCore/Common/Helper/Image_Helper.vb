Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Public Class Image_Helper
    'TODO: FIX HERE
    Public Shared Function AddWaterMark(ByVal stream As Stream,
                                        ByVal pExtendHeight As Boolean,
                                        ByVal waterMark As MemoryStream) As Image
        'Dim image__1 As Image = Image.FromStream(stream)
        'Dim imgWidth As Integer = image__1.Width
        'Dim imgHeight As Integer = image__1.Height
        'Dim newWidth As Integer = imgWidth
        'Dim newHeight As Integer = imgHeight
        'If pExtendHeight Then
        '    newHeight += 20
        'End If
        'Dim thumbnail As Image = New Bitmap(newWidth, newHeight)
        'Dim graphics__2 As Graphics = Graphics.FromImage(thumbnail)
        'graphics__2.Clear(Color.White)
        'graphics__2.InterpolationMode = InterpolationMode.HighQualityBicubic
        'graphics__2.SmoothingMode = SmoothingMode.HighQuality
        'graphics__2.CompositingQuality = CompositingQuality.HighQuality
        'graphics__2.PixelOffsetMode = PixelOffsetMode.HighQuality
        'graphics__2.DrawImage(image__1, 0, 0, imgWidth, imgHeight)
        'Using wm As Image = CType(Image.FromStream(waterMark).Clone, Image)
        '    Dim wmWidthPos As Integer = CInt(newWidth - wm.Width)
        '    Dim wmHeightPos As Integer = CInt(newHeight - wm.Height)
        '    graphics__2.DrawImage(wm, wmWidthPos, wmHeightPos, wm.Width, wm.Height)
        'End Using

        'Dim encoderParameters As New EncoderParameters(1)
        'encoderParameters.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L)
        'Dim ms As New MemoryStream
        'thumbnail.Save(ms, GetEncoder(image__1.RawFormat), encoderParameters)
        'Return Image.FromStream(ms)
        Return Nothing
    End Function
    Private Shared Function GetEncoder(format As ImageFormat) As ImageCodecInfo
        'Dim codecs As ImageCodecInfo() = ImageCodecInfo.GetImageDecoders()
        'For Each codec As ImageCodecInfo In codecs
        '    If codec.FormatID = format.Guid Then
        '        Return codec
        '    End If
        'Next
        Return Nothing
    End Function
End Class
