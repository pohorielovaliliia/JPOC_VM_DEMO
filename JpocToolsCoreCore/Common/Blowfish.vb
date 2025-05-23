Imports System.Security.Cryptography
Imports System.Text
#Region "Blowfish暗号化クラス"

Public Class Blowfish
    'Blowfish encryption (ECB and CBC MODE) as defined by Bruce Schneier here: http://www.schneier.com/paper-blowfish-fse.html
    'Complies with test vectors found here: http://www.schneier.com/code/vectors.txt
    'non-standard mode profided to be usable with the javascript crypto library found here: http://etherhack.co.uk/symmetric/blowfish/blowfish.html
    'By FireXware, 1/7/1010, Contact: firexware@hotmail.com
    'Code is partly adopted from the javascript crypto library by Daniel Rench

    'USAGE:
    'BlowFish b = new BlowFish("04B915BA43FEB5B6");
    'string plainText = "The quick brown fox jumped over the lazy dog.";
    'string cipherText = b.Encrypt_CBC(plainText);
    'MessageBox.Show(cipherText);
    'plainText = b.Decrypt_CBC(cipherText);
    'MessageBox.Show(plainText);

    'TODO: FIX HERE
    'Private randomSource As RNGCryptoServiceProvider

    'SBLOCKS
    Private bf_s0 As UInteger()
    Private bf_s1 As UInteger()
    Private bf_s2 As UInteger()
    Private bf_s3 As UInteger()

    Private bf_P As UInteger()

    'KEY
    Private key As Byte()

    'HALF-BLOCKS
    Private xl_par As UInteger
    Private xr_par As UInteger

    Private InitVector As Byte()
    Private IVSet As Boolean

    'COMPATIBILITY WITH javascript CRYPTO LIBRARY
    Private nonStandardMethod As Boolean

    ''' <summary>
    ''' Constructor for hex key
    ''' </summary>
    ''' <param name="hexKey">Cipher key as a hex string</param>
    Public Sub New(hexKey As String)
        'randomSource = New RNGCryptoServiceProvider()
        SetupKey(HexToByte(hexKey))
    End Sub

    ''' <summary>
    ''' Constructor for byte key
    ''' </summary>
    ''' <param name="cipherKey">Cipher key as a byte array</param>
    Public Sub New(cipherKey As Byte())
        'randomSource = New RNGCryptoServiceProvider()
        SetupKey(cipherKey)
    End Sub

    ''' <summary>
    ''' Encrypts a string in CBC mode
    ''' </summary>
    ''' <param name="pt">Plaintext data to encrypt</param>
    ''' <returns>Ciphertext with IV appended to front</returns>
    Public Function Encrypt_CBC(pt As String) As String
        If Not IVSet Then
            SetRandomIV()
        End If
        Return ByteToHex(InitVector) & ByteToHex(Encrypt_CBC(Encoding.ASCII.GetBytes(pt)))
    End Function

    ''' <summary>
    ''' Decrypts a string in CBC mode
    ''' </summary>
    ''' <param name="ct">Ciphertext with IV appended to front</param>
    ''' <returns>Plaintext</returns>
    Public Function Decrypt_CBC(ct As String) As String
        IV = HexToByte(ct.Substring(0, 16))
        Return Encoding.ASCII.GetString(Decrypt_CBC(HexToByte(ct.Substring(16)))).Replace(vbNullChar, "")
    End Function

    ''' <summary>
    ''' Decrypts a byte array in CBC mode.
    ''' IV must be created and saved manually.
    ''' </summary>
    ''' <param name="ct">Ciphertext data to decrypt</param>
    ''' <returns>Plaintext</returns>
    Public Function Decrypt_CBC(ct As Byte()) As Byte()
        Return Crypt_CBC(ct, True)
    End Function

    ''' <summary>
    ''' Encrypts a byte array in CBC mode.
    ''' IV must be created and saved manually.
    ''' </summary>
    ''' <param name="pt">Plaintext data to encrypt</param>
    ''' <returns>Ciphertext</returns>
    Public Function Encrypt_CBC(pt As Byte()) As Byte()
        Return Crypt_CBC(pt, False)
    End Function

    ''' <summary>
    ''' Encrypt a string in ECB mode
    ''' </summary>
    ''' <param name="pt">Plaintext to encrypt as ascii string</param>
    ''' <returns>hex value of encrypted data</returns>
    Public Function Encrypt_ECB(pt As String) As String
        Return ByteToHex(Encrypt_ECB(Encoding.ASCII.GetBytes(pt)))
    End Function

    ''' <summary>
    ''' Decrypts a string (ECB)
    ''' </summary>
    ''' <param name="ct">hHex string of the ciphertext</param>
    ''' <returns>Plaintext ascii string</returns>
    Public Function Decrypt_ECB(ct As String) As String
        Return Encoding.ASCII.GetString(Decrypt_ECB(HexToByte(ct))).Replace(vbNullChar, "")
    End Function

    ''' <summary>
    ''' Encrypts a byte array in ECB mode
    ''' </summary>
    ''' <param name="pt">Plaintext data</param>
    ''' <returns>Ciphertext bytes</returns>
    Public Function Encrypt_ECB(pt As Byte()) As Byte()
        Return Crypt_ECB(pt, False)
    End Function

    ''' <summary>
    ''' Decrypts a byte array (ECB)
    ''' </summary>
    ''' <param name="ct">Ciphertext byte array</param>
    ''' <returns>Plaintext</returns>
    Public Function Decrypt_ECB(ct As Byte()) As Byte()
        Return Crypt_ECB(ct, True)
    End Function

    ''' <summary>
    ''' Initialization vector for CBC mode.
    ''' </summary>
    Public Property IV() As Byte()
        Get
            Return InitVector
        End Get
        Set(value As Byte())
            If value.Length = 8 Then
                InitVector = value
                IVSet = True
            Else
                Throw New Exception("Invalid IV size.")
            End If
        End Set
    End Property

    Public Property NonStandard() As Boolean
        Get
            Return nonStandardMethod
        End Get
        Set(value As Boolean)
            nonStandardMethod = value
        End Set
    End Property

    ''' <summary>
    ''' Creates and sets a random initialization vector.
    ''' </summary>
    ''' <returns>The random IV</returns>
    Public Function SetRandomIV() As Byte()
        InitVector = New Byte(7) {}
        'randomSource.GetBytes(InitVector)
        IVSet = True
        Return InitVector
    End Function

#Region "Cryptography"

    ''' <summary>
    ''' Sets up the S-blocks and the key
    ''' </summary>
    ''' <param name="cipherKey">Block cipher key (1-448 bits)</param>
    Private Sub SetupKey(cipherKey As Byte())
        bf_P = SetupP()
        'set up the S blocks
        bf_s0 = SetupS0()
        bf_s1 = SetupS1()
        bf_s2 = SetupS2()
        bf_s3 = SetupS3()

        key = New Byte(cipherKey.Length - 1) {}
        ' 448 bits
        If cipherKey.Length > 56 Then
            Throw New Exception("Key too long. 56 bytes required.")
        End If

        Buffer.BlockCopy(cipherKey, 0, key, 0, cipherKey.Length)
        Dim j As Integer = 0
        For i As Integer = 0 To 17
            Dim d As UInteger = CUInt(((key(j Mod cipherKey.Length) * 256 + key((j + 1) Mod cipherKey.Length)) * 256 + key((j + 2) Mod cipherKey.Length)) * 256 + key((j + 3) Mod cipherKey.Length))
            bf_P(i) = bf_P(i) Xor d
            j = (j + 4) Mod cipherKey.Length
        Next

        xl_par = 0
        xr_par = 0
        For i As Integer = 0 To 17 Step 2
            encipher()
            bf_P(i) = xl_par
            bf_P(i + 1) = xr_par
        Next

        For i As Integer = 0 To 255 Step 2
            encipher()
            bf_s0(i) = xl_par
            bf_s0(i + 1) = xr_par
        Next
        For i As Integer = 0 To 255 Step 2
            encipher()
            bf_s1(i) = xl_par
            bf_s1(i + 1) = xr_par
        Next
        For i As Integer = 0 To 255 Step 2
            encipher()
            bf_s2(i) = xl_par
            bf_s2(i + 1) = xr_par
        Next
        For i As Integer = 0 To 255 Step 2
            encipher()
            bf_s3(i) = xl_par
            bf_s3(i + 1) = xr_par
        Next
    End Sub

    ''' <summary>
    ''' Encrypts or decrypts data in ECB mode
    ''' </summary>
    ''' <param name="text">plain/ciphertext</param>
    ''' <param name="decrypt">true to decrypt, false to encrypt</param>
    ''' <returns>(En/De)crypted data</returns>
    Private Function Crypt_ECB(text As Byte(), decrypt As Boolean) As Byte()
        Dim paddedLen As Integer = (If(text.Length Mod 8 = 0, text.Length, text.Length + 8 - (text.Length Mod 8)))
        Dim plainText As Byte() = New Byte(paddedLen - 1) {}
        Buffer.BlockCopy(text, 0, plainText, 0, text.Length)
        Dim block As Byte() = New Byte(7) {}
        For i As Integer = 0 To plainText.Length - 1 Step 8
            Buffer.BlockCopy(plainText, i, block, 0, 8)
            If decrypt Then
                BlockDecrypt(block)
            Else
                BlockEncrypt(block)
            End If
            Buffer.BlockCopy(block, 0, plainText, i, 8)
        Next
        Return plainText
    End Function

    ''' <summary>
    ''' Encrypts or decrypts data in CBC mode
    ''' </summary>
    ''' <param name="text">plain/ciphertext</param>
    ''' <param name="decrypt">true to decrypt, false to encrypt</param>
    ''' <returns>(En/De)crypted data</returns>
    Private Function Crypt_CBC(text As Byte(), decrypt As Boolean) As Byte()
        If Not IVSet Then
            Throw New Exception("IV not set.")
        End If
        Dim paddedLen As Integer = (If(text.Length Mod 8 = 0, text.Length, text.Length + 8 - (text.Length Mod 8)))
        Dim plainText As Byte() = New Byte(paddedLen - 1) {}
        Buffer.BlockCopy(text, 0, plainText, 0, text.Length)
        Dim block As Byte() = New Byte(7) {}
        Dim preblock As Byte() = New Byte(7) {}
        Dim iv As Byte() = New Byte(7) {}
        Buffer.BlockCopy(InitVector, 0, iv, 0, 8)
        If Not decrypt Then
            For i As Integer = 0 To plainText.Length - 1 Step 8
                Buffer.BlockCopy(plainText, i, block, 0, 8)
                XorBlock(block, iv)
                BlockEncrypt(block)
                Buffer.BlockCopy(block, 0, iv, 0, 8)
                Buffer.BlockCopy(block, 0, plainText, i, 8)
            Next
        Else
            For i As Integer = 0 To plainText.Length - 1 Step 8
                Buffer.BlockCopy(plainText, i, block, 0, 8)

                Buffer.BlockCopy(block, 0, preblock, 0, 8)
                BlockDecrypt(block)
                XorBlock(block, iv)
                Buffer.BlockCopy(preblock, 0, iv, 0, 8)

                Buffer.BlockCopy(block, 0, plainText, i, 8)
            Next
        End If
        Return plainText
    End Function

    ''' <summary>
    ''' XoR encrypts two 8 bit blocks
    ''' </summary>
    ''' <param name="block">8 bit block 1</param>
    ''' <param name="iv">8 bit block 2</param>
    Private Sub XorBlock(ByRef block As Byte(), iv As Byte())
        For i As Integer = 0 To block.Length - 1
            block(i) = block(i) Xor iv(i)
        Next
    End Sub

    ''' <summary>
    ''' Encrypts a 64 bit block
    ''' </summary>
    ''' <param name="block">The 64 bit block to encrypt</param>
    Private Sub BlockEncrypt(ByRef block As Byte())
        SetBlock(block)
        encipher()
        GetBlock(block)
    End Sub

    ''' <summary>
    ''' Decrypts a 64 bit block
    ''' </summary>
    ''' <param name="block">The 64 bit block to decrypt</param>
    Private Sub BlockDecrypt(ByRef block As Byte())
        SetBlock(block)
        decipher()
        GetBlock(block)
    End Sub

    ''' <summary>
    ''' Splits the block into the two uint values
    ''' </summary>
    ''' <param name="block">the 64 bit block to setup</param>
    Private Sub SetBlock(block As Byte())
        Dim block1 As Byte() = New Byte(3) {}
        Dim block2 As Byte() = New Byte(3) {}
        Buffer.BlockCopy(block, 0, block1, 0, 4)
        Buffer.BlockCopy(block, 4, block2, 0, 4)
        'split the block
        If nonStandardMethod Then
            xr_par = BitConverter.ToUInt32(block1, 0)
            xl_par = BitConverter.ToUInt32(block2, 0)
        Else
            'ToUInt32 requires the bytes in reverse order
            Array.Reverse(block1)
            Array.Reverse(block2)
            xl_par = BitConverter.ToUInt32(block1, 0)
            xr_par = BitConverter.ToUInt32(block2, 0)
        End If
    End Sub

    ''' <summary>
    ''' Converts the two uint values into a 64 bit block
    ''' </summary>
    ''' <param name="block">64 bit buffer to receive the block</param>
    Private Sub GetBlock(ByRef block As Byte())
        Dim block1 As Byte() = New Byte(3) {}
        Dim block2 As Byte() = New Byte(3) {}
        If nonStandardMethod Then
            block1 = BitConverter.GetBytes(xr_par)
            block2 = BitConverter.GetBytes(xl_par)
        Else
            block1 = BitConverter.GetBytes(xl_par)
            block2 = BitConverter.GetBytes(xr_par)

            'GetBytes returns the bytes in reverse order
            Array.Reverse(block1)
            Array.Reverse(block2)
        End If
        'join the block
        Buffer.BlockCopy(block1, 0, block, 0, 4)
        Buffer.BlockCopy(block2, 0, block, 4, 4)
    End Sub

    ''' <summary>
    ''' Runs the blowfish algorithm (standard 16 rounds)
    ''' </summary>
    Private Sub encipher()
        xl_par = xl_par Xor bf_P(0)
        For i As UInteger = 0 To 15 Step 2
            xr_par = round(xr_par, xl_par, CUInt(i + 1))
            xl_par = round(xl_par, xr_par, CUInt(i + 2))
        Next
        xr_par = xr_par Xor bf_P(17)

        'swap the blocks
        Dim swap As UInteger = xl_par
        xl_par = xr_par
        xr_par = swap
    End Sub

    ''' <summary>
    ''' Runs the blowfish algorithm in reverse (standard 16 rounds)
    ''' </summary>
    Private Sub decipher()
        xl_par = xl_par Xor bf_P(17)
        For i As Integer = 16 To 1 Step -2
            xr_par = round(xr_par, xl_par, CUInt(i))
            xl_par = round(xl_par, xr_par, CUInt(i - 1))
        Next
        xr_par = xr_par Xor bf_P(0)

        'swap the blocks
        Dim swap As UInteger = xl_par
        xl_par = xr_par
        xr_par = swap
    End Sub

    ''' <summary>
    ''' one round of the blowfish algorithm
    ''' </summary>
    ''' <param name="a">See spec</param>
    ''' <param name="b">See spec</param>
    ''' <param name="n">See spec</param>
    ''' <returns></returns>
    Private Function round(ByVal a As UInteger, ByVal b As UInteger, ByVal n As UInteger) As UInteger
        'Dim x1 As UInteger = (bf_s0(wordByte0(b)) + bf_s1(wordByte1(b))) Xor bf_s2(wordByte2(b))
        'Dim x2 As UInteger = x1 + bf_s3(Me.wordByte3(b))
        Dim x1 As UInt64 = (Convert.ToUInt64(bf_s0(wordByte0(b))) + Convert.ToUInt64(bf_s1(wordByte1(b)))) Xor Convert.ToUInt64(bf_s2(wordByte2(b)))
        Dim x2 As UInt64 = x1 + bf_s3(Me.wordByte3(b))
        Dim x3 As UInteger = Convert.ToUInt32((x2 Xor bf_P(CInt(n))) And &HFFFFFFFFUL)
        Return x3 Xor a
    End Function
#End Region

#Region "SBLOCKS"
    'SBLOCKS ARE THE HEX DIGITS OF PI. 
    'The amount of hex digits can be increased if you want to experiment with more rounds and longer key lengths
    Private Function SetupP() As UInteger()
        Return New UInteger() { _
         &H243F6A88UI, &H85A308D3UI, &H13198A2EUI, &H3707344UI, &HA4093822UI, &H299F31D0UI, _
         &H82EFA98UI, &HEC4E6C89UI, &H452821E6UI, &H38D01377UI, &HBE5466CFUI, &H34E90C6CUI, _
         &HC0AC29B7UI, &HC97C50DDUI, &H3F84D5B5UI, &HB5470917UI, &H9216D5D9UI, &H8979FB1BUI}
    End Function

    Private Function SetupS0() As UInteger()
        Return New UInteger() { _
         &HD1310BA6UI, &H98DFB5ACUI, &H2FFD72DBUI, &HD01ADFB7UI, &HB8E1AFEDUI, &H6A267E96UI, _
         &HBA7C9045UI, &HF12C7F99UI, &H24A19947UI, &HB3916CF7UI, &H801F2E2UI, &H858EFC16UI, _
         &H636920D8UI, &H71574E69UI, &HA458FEA3UI, &HF4933D7EUI, &HD95748FUI, &H728EB658UI, _
         &H718BCD58UI, &H82154AEEUI, &H7B54A41DUI, &HC25A59B5UI, &H9C30D539UI, &H2AF26013UI, _
         &HC5D1B023UI, &H286085F0UI, &HCA417918UI, &HB8DB38EFUI, &H8E79DCB0UI, &H603A180EUI, _
         &H6C9E0E8BUI, &HB01E8A3EUI, &HD71577C1UI, &HBD314B27UI, &H78AF2FDAUI, &H55605C60UI, _
         &HE65525F3UI, &HAA55AB94UI, &H57489862UI, &H63E81440UI, &H55CA396AUI, &H2AAB10B6UI, _
         &HB4CC5C34UI, &H1141E8CEUI, &HA15486AFUI, &H7C72E993UI, &HB3EE1411UI, &H636FBC2AUI, _
         &H2BA9C55DUI, &H741831F6UI, &HCE5C3E16UI, &H9B87931EUI, &HAFD6BA33UI, &H6C24CF5CUI, _
         &H7A325381UI, &H28958677UI, &H3B8F4898UI, &H6B4BB9AFUI, &HC4BFE81BUI, &H66282193UI, _
         &H61D809CCUI, &HFB21A991UI, &H487CAC60UI, &H5DEC8032UI, &HEF845D5DUI, &HE98575B1UI, _
         &HDC262302UI, &HEB651B88UI, &H23893E81UI, &HD396ACC5UI, &HF6D6FF3UI, &H83F44239UI, _
         &H2E0B4482UI, &HA4842004UI, &H69C8F04AUI, &H9E1F9B5EUI, &H21C66842UI, &HF6E96C9AUI, _
         &H670C9C61UI, &HABD388F0UI, &H6A51A0D2UI, &HD8542F68UI, &H960FA728UI, &HAB5133A3UI, _
         &H6EEF0B6CUI, &H137A3BE4UI, &HBA3BF050UI, &H7EFB2A98UI, &HA1F1651DUI, &H39AF0176UI, _
         &H66CA593EUI, &H82430E88UI, &H8CEE8619UI, &H456F9FB4UI, &H7D84A5C3UI, &H3B8B5EBEUI, _
         &HE06F75D8UI, &H85C12073UI, &H401A449FUI, &H56C16AA6UI, &H4ED3AA62UI, &H363F7706UI, _
         &H1BFEDF72UI, &H429B023DUI, &H37D0D724UI, &HD00A1248UI, &HDB0FEAD3UI, &H49F1C09BUI, _
         &H75372C9UI, &H80991B7BUI, &H25D479D8UI, &HF6E8DEF7UI, &HE3FE501AUI, &HB6794C3BUI, _
         &H976CE0BDUI, &H4C006BAUI, &HC1A94FB6UI, &H409F60C4UI, &H5E5C9EC2UI, &H196A2463UI, _
         &H68FB6FAFUI, &H3E6C53B5UI, &H1339B2EBUI, &H3B52EC6FUI, &H6DFC511FUI, &H9B30952CUI, _
         &HCC814544UI, &HAF5EBD09UI, &HBEE3D004UI, &HDE334AFDUI, &H660F2807UI, &H192E4BB3UI, _
         &HC0CBA857UI, &H45C8740FUI, &HD20B5F39UI, &HB9D3FBDBUI, &H5579C0BDUI, &H1A60320AUI, _
         &HD6A100C6UI, &H402C7279UI, &H679F25FEUI, &HFB1FA3CCUI, &H8EA5E9F8UI, &HDB3222F8UI, _
         &H3C7516DFUI, &HFD616B15UI, &H2F501EC8UI, &HAD0552ABUI, &H323DB5FAUI, &HFD238760UI, _
         &H53317B48UI, &H3E00DF82UI, &H9E5C57BBUI, &HCA6F8CA0UI, &H1A87562EUI, &HDF1769DBUI, _
         &HD542A8F6UI, &H287EFFC3UI, &HAC6732C6UI, &H8C4F5573UI, &H695B27B0UI, &HBBCA58C8UI, _
         &HE1FFA35DUI, &HB8F011A0UI, &H10FA3D98UI, &HFD2183B8UI, &H4AFCB56CUI, &H2DD1D35BUI, _
         &H9A53E479UI, &HB6F84565UI, &HD28E49BCUI, &H4BFB9790UI, &HE1DDF2DAUI, &HA4CB7E33UI, _
         &H62FB1341UI, &HCEE4C6E8UI, &HEF20CADAUI, &H36774C01UI, &HD07E9EFEUI, &H2BF11FB4UI, _
         &H95DBDA4DUI, &HAE909198UI, &HEAAD8E71UI, &H6B93D5A0UI, &HD08ED1D0UI, &HAFC725E0UI, _
         &H8E3C5B2FUI, &H8E7594B7UI, &H8FF6E2FBUI, &HF2122B64UI, &H8888B812UI, &H900DF01CUI, _
         &H4FAD5EA0UI, &H688FC31CUI, &HD1CFF191UI, &HB3A8C1ADUI, &H2F2F2218UI, &HBE0E1777UI, _
         &HEA752DFEUI, &H8B021FA1UI, &HE5A0CC0FUI, &HB56F74E8UI, &H18ACF3D6UI, &HCE89E299UI, _
         &HB4A84FE0UI, &HFD13E0B7UI, &H7CC43B81UI, &HD2ADA8D9UI, &H165FA266UI, &H80957705UI, _
         &H93CC7314UI, &H211A1477UI, &HE6AD2065UI, &H77B5FA86UI, &HC75442F5UI, &HFB9D35CFUI, _
         &HEBCDAF0CUI, &H7B3E89A0UI, &HD6411BD3UI, &HAE1E7E49UI, &H250E2DUI, &H2071B35EUI, _
         &H226800BBUI, &H57B8E0AFUI, &H2464369BUI, &HF009B91EUI, &H5563911DUI, &H59DFA6AAUI, _
         &H78C14389UI, &HD95A537FUI, &H207D5BA2UI, &H2E5B9C5UI, &H83260376UI, &H6295CFA9UI, _
         &H11C81968UI, &H4E734A41UI, &HB3472DCAUI, &H7B14A94AUI, &H1B510052UI, &H9A532915UI, _
         &HD60F573FUI, &HBC9BC6E4UI, &H2B60A476UI, &H81E67400UI, &H8BA6FB5UI, &H571BE91FUI, _
         &HF296EC6BUI, &H2A0DD915UI, &HB6636521UI, &HE7B9F9B6UI, &HFF34052EUI, &HC5855664UI, _
         &H53B02D5DUI, &HA99F8FA1UI, &H8BA4799UI, &H6E85076AUI}
    End Function

    Private Function SetupS1() As UInteger()
        Return New UInteger() { _
         &H4B7A70E9UI, &HB5B32944UI, &HDB75092EUI, &HC4192623UI, &HAD6EA6B0UI, &H49A7DF7DUI, _
         &H9CEE60B8UI, &H8FEDB266UI, &HECAA8C71UI, &H699A17FFUI, &H5664526CUI, &HC2B19EE1UI, _
         &H193602A5UI, &H75094C29UI, &HA0591340UI, &HE4183A3EUI, &H3F54989AUI, &H5B429D65UI, _
         &H6B8FE4D6UI, &H99F73FD6UI, &HA1D29C07UI, &HEFE830F5UI, &H4D2D38E6UI, &HF0255DC1UI, _
         &H4CDD2086UI, &H8470EB26UI, &H6382E9C6UI, &H21ECC5EUI, &H9686B3FUI, &H3EBAEFC9UI, _
         &H3C971814UI, &H6B6A70A1UI, &H687F3584UI, &H52A0E286UI, &HB79C5305UI, &HAA500737UI, _
         &H3E07841CUI, &H7FDEAE5CUI, &H8E7D44ECUI, &H5716F2B8UI, &HB03ADA37UI, &HF0500C0DUI, _
         &HF01C1F04UI, &H200B3FFUI, &HAE0CF51AUI, &H3CB574B2UI, &H25837A58UI, &HDC0921BDUI, _
         &HD19113F9UI, &H7CA92FF6UI, &H94324773UI, &H22F54701UI, &H3AE5E581UI, &H37C2DADCUI, _
         &HC8B57634UI, &H9AF3DDA7UI, &HA9446146UI, &HFD0030EUI, &HECC8C73EUI, &HA4751E41UI, _
         &HE238CD99UI, &H3BEA0E2FUI, &H3280BBA1UI, &H183EB331UI, &H4E548B38UI, &H4F6DB908UI, _
         &H6F420D03UI, &HF60A04BFUI, &H2CB81290UI, &H24977C79UI, &H5679B072UI, &HBCAF89AFUI, _
         &HDE9A771FUI, &HD9930810UI, &HB38BAE12UI, &HDCCF3F2EUI, &H5512721FUI, &H2E6B7124UI, _
         &H501ADDE6UI, &H9F84CD87UI, &H7A584718UI, &H7408DA17UI, &HBC9F9ABCUI, &HE94B7D8CUI, _
         &HEC7AEC3AUI, &HDB851DFAUI, &H63094366UI, &HC464C3D2UI, &HEF1C1847UI, &H3215D908UI, _
         &HDD433B37UI, &H24C2BA16UI, &H12A14D43UI, &H2A65C451UI, &H50940002UI, &H133AE4DDUI, _
         &H71DFF89EUI, &H10314E55UI, &H81AC77D6UI, &H5F11199BUI, &H43556F1UI, &HD7A3C76BUI, _
         &H3C11183BUI, &H5924A509UI, &HF28FE6EDUI, &H97F1FBFAUI, &H9EBABF2CUI, &H1E153C6EUI, _
         &H86E34570UI, &HEAE96FB1UI, &H860E5E0AUI, &H5A3E2AB3UI, &H771FE71CUI, &H4E3D06FAUI, _
         &H2965DCB9UI, &H99E71D0FUI, &H803E89D6UI, &H5266C825UI, &H2E4CC978UI, &H9C10B36AUI, _
         &HC6150EBAUI, &H94E2EA78UI, &HA5FC3C53UI, &H1E0A2DF4UI, &HF2F74EA7UI, &H361D2B3DUI, _
         &H1939260FUI, &H19C27960UI, &H5223A708UI, &HF71312B6UI, &HEBADFE6EUI, &HEAC31F66UI, _
         &HE3BC4595UI, &HA67BC883UI, &HB17F37D1UI, &H18CFF28UI, &HC332DDEFUI, &HBE6C5AA5UI, _
         &H65582185UI, &H68AB9802UI, &HEECEA50FUI, &HDB2F953BUI, &H2AEF7DADUI, &H5B6E2F84UI, _
         &H1521B628UI, &H29076170UI, &HECDD4775UI, &H619F1510UI, &H13CCA830UI, &HEB61BD96UI, _
         &H334FE1EUI, &HAA0363CFUI, &HB5735C90UI, &H4C70A239UI, &HD59E9E0BUI, &HCBAADE14UI, _
         &HEECC86BCUI, &H60622CA7UI, &H9CAB5CABUI, &HB2F3846EUI, &H648B1EAFUI, &H19BDF0CAUI, _
         &HA02369B9UI, &H655ABB50UI, &H40685A32UI, &H3C2AB4B3UI, &H319EE9D5UI, &HC021B8F7UI, _
         &H9B540B19UI, &H875FA099UI, &H95F7997EUI, &H623D7DA8UI, &HF837889AUI, &H97E32D77UI, _
         &H11ED935FUI, &H16681281UI, &HE358829UI, &HC7E61FD6UI, &H96DEDFA1UI, &H7858BA99UI, _
         &H57F584A5UI, &H1B227263UI, &H9B83C3FFUI, &H1AC24696UI, &HCDB30AEBUI, &H532E3054UI, _
         &H8FD948E4UI, &H6DBC3128UI, &H58EBF2EFUI, &H34C6FFEAUI, &HFE28ED61UI, &HEE7C3C73UI, _
         &H5D4A14D9UI, &HE864B7E3UI, &H42105D14UI, &H203E13E0UI, &H45EEE2B6UI, &HA3AAABEAUI, _
         &HDB6C4F15UI, &HFACB4FD0UI, &HC742F442UI, &HEF6ABBB5UI, &H654F3B1DUI, &H41CD2105UI, _
         &HD81E799EUI, &H86854DC7UI, &HE44B476AUI, &H3D816250UI, &HCF62A1F2UI, &H5B8D2646UI, _
         &HFC8883A0UI, &HC1C7B6A3UI, &H7F1524C3UI, &H69CB7492UI, &H47848A0BUI, &H5692B285UI, _
         &H95BBF00UI, &HAD19489DUI, &H1462B174UI, &H23820E00UI, &H58428D2AUI, &HC55F5EAUI, _
         &H1DADF43EUI, &H233F7061UI, &H3372F092UI, &H8D937E41UI, &HD65FECF1UI, &H6C223BDBUI, _
         &H7CDE3759UI, &HCBEE7460UI, &H4085F2A7UI, &HCE77326EUI, &HA6078084UI, &H19F8509EUI, _
         &HE8EFD855UI, &H61D99735UI, &HA969A7AAUI, &HC50C06C2UI, &H5A04ABFCUI, &H800BCADCUI, _
         &H9E447A2EUI, &HC3453484UI, &HFDD56705UI, &HE1E9EC9UI, &HDB73DBD3UI, &H105588CDUI, _
         &H675FDA79UI, &HE3674340UI, &HC5C43465UI, &H713E38D8UI, &H3D28F89EUI, &HF16DFF20UI, _
         &H153E21E7UI, &H8FB03D4AUI, &HE6E39F2BUI, &HDB83ADF7UI}
    End Function

    Private Function SetupS2() As UInteger()
        Return New UInteger() { _
         &HE93D5A68UI, &H948140F7UI, &HF64C261CUI, &H94692934UI, &H411520F7UI, &H7602D4F7UI, _
         &HBCF46B2EUI, &HD4A20068UI, &HD4082471UI, &H3320F46AUI, &H43B7D4B7UI, &H500061AFUI, _
         &H1E39F62EUI, &H97244546UI, &H14214F74UI, &HBF8B8840UI, &H4D95FC1DUI, &H96B591AFUI, _
         &H70F4DDD3UI, &H66A02F45UI, &HBFBC09ECUI, &H3BD9785UI, &H7FAC6DD0UI, &H31CB8504UI, _
         &H96EB27B3UI, &H55FD3941UI, &HDA2547E6UI, &HABCA0A9AUI, &H28507825UI, &H530429F4UI, _
         &HA2C86DAUI, &HE9B66DFBUI, &H68DC1462UI, &HD7486900UI, &H680EC0A4UI, &H27A18DEEUI, _
         &H4F3FFEA2UI, &HE887AD8CUI, &HB58CE006UI, &H7AF4D6B6UI, &HAACE1E7CUI, &HD3375FECUI, _
         &HCE78A399UI, &H406B2A42UI, &H20FE9E35UI, &HD9F385B9UI, &HEE39D7ABUI, &H3B124E8BUI, _
         &H1DC9FAF7UI, &H4B6D1856UI, &H26A36631UI, &HEAE397B2UI, &H3A6EFA74UI, &HDD5B4332UI, _
         &H6841E7F7UI, &HCA7820FBUI, &HFB0AF54EUI, &HD8FEB397UI, &H454056ACUI, &HBA489527UI, _
         &H55533A3AUI, &H20838D87UI, &HFE6BA9B7UI, &HD096954BUI, &H55A867BCUI, &HA1159A58UI, _
         &HCCA92963UI, &H99E1DB33UI, &HA62A4A56UI, &H3F3125F9UI, &H5EF47E1CUI, &H9029317CUI, _
         &HFDF8E802UI, &H4272F70UI, &H80BB155CUI, &H5282CE3UI, &H95C11548UI, &HE4C66D22UI, _
         &H48C1133FUI, &HC70F86DCUI, &H7F9C9EEUI, &H41041F0FUI, &H404779A4UI, &H5D886E17UI, _
         &H325F51EBUI, &HD59BC0D1UI, &HF2BCC18FUI, &H41113564UI, &H257B7834UI, &H602A9C60UI, _
         &HDFF8E8A3UI, &H1F636C1BUI, &HE12B4C2UI, &H2E1329EUI, &HAF664FD1UI, &HCAD18115UI, _
         &H6B2395E0UI, &H333E92E1UI, &H3B240B62UI, &HEEBEB922UI, &H85B2A20EUI, &HE6BA0D99UI, _
         &HDE720C8CUI, &H2DA2F728UI, &HD0127845UI, &H95B794FDUI, &H647D0862UI, &HE7CCF5F0UI, _
         &H5449A36FUI, &H877D48FAUI, &HC39DFD27UI, &HF33E8D1EUI, &HA476341UI, &H992EFF74UI, _
         &H3A6F6EABUI, &HF4F8FD37UI, &HA812DC60UI, &HA1EBDDF8UI, &H991BE14CUI, &HDB6E6B0DUI, _
         &HC67B5510UI, &H6D672C37UI, &H2765D43BUI, &HDCD0E804UI, &HF1290DC7UI, &HCC00FFA3UI, _
         &HB5390F92UI, &H690FED0BUI, &H667B9FFBUI, &HCEDB7D9CUI, &HA091CF0BUI, &HD9155EA3UI, _
         &HBB132F88UI, &H515BAD24UI, &H7B9479BFUI, &H763BD6EBUI, &H37392EB3UI, &HCC115979UI, _
         &H8026E297UI, &HF42E312DUI, &H6842ADA7UI, &HC66A2B3BUI, &H12754CCCUI, &H782EF11CUI, _
         &H6A124237UI, &HB79251E7UI, &H6A1BBE6UI, &H4BFB6350UI, &H1A6B1018UI, &H11CAEDFAUI, _
         &H3D25BDD8UI, &HE2E1C3C9UI, &H44421659UI, &HA121386UI, &HD90CEC6EUI, &HD5ABEA2AUI, _
         &H64AF674EUI, &HDA86A85FUI, &HBEBFE988UI, &H64E4C3FEUI, &H9DBC8057UI, &HF0F7C086UI, _
         &H60787BF8UI, &H6003604DUI, &HD1FD8346UI, &HF6381FB0UI, &H7745AE04UI, &HD736FCCCUI, _
         &H83426B33UI, &HF01EAB71UI, &HB0804187UI, &H3C005E5FUI, &H77A057BEUI, &HBDE8AE24UI, _
         &H55464299UI, &HBF582E61UI, &H4E58F48FUI, &HF2DDFDA2UI, &HF474EF38UI, &H8789BDC2UI, _
         &H5366F9C3UI, &HC8B38E74UI, &HB475F255UI, &H46FCD9B9UI, &H7AEB2661UI, &H8B1DDF84UI, _
         &H846A0E79UI, &H915F95E2UI, &H466E598EUI, &H20B45770UI, &H8CD55591UI, &HC902DE4CUI, _
         &HB90BACE1UI, &HBB8205D0UI, &H11A86248UI, &H7574A99EUI, &HB77F19B6UI, &HE0A9DC09UI, _
         &H662D09A1UI, &HC4324633UI, &HE85A1F02UI, &H9F0BE8CUI, &H4A99A025UI, &H1D6EFE10UI, _
         &H1AB93D1DUI, &HBA5A4DFUI, &HA186F20FUI, &H2868F169UI, &HDCB7DA83UI, &H573906FEUI, _
         &HA1E2CE9BUI, &H4FCD7F52UI, &H50115E01UI, &HA70683FAUI, &HA002B5C4UI, &HDE6D027UI, _
         &H9AF88C27UI, &H773F8641UI, &HC3604C06UI, &H61A806B5UI, &HF0177A28UI, &HC0F586E0UI, _
         &H6058AAUI, &H30DC7D62UI, &H11E69ED7UI, &H2338EA63UI, &H53C2DD94UI, &HC2C21634UI, _
         &HBBCBEE56UI, &H90BCB6DEUI, &HEBFC7DA1UI, &HCE591D76UI, &H6F05E409UI, &H4B7C0188UI, _
         &H39720A3DUI, &H7C927C24UI, &H86E3725FUI, &H724D9DB9UI, &H1AC15BB4UI, &HD39EB8FCUI, _
         &HED545578UI, &H8FCA5B5UI, &HD83D7CD3UI, &H4DAD0FC4UI, &H1E50EF5EUI, &HB161E6F8UI, _
         &HA28514D9UI, &H6C51133CUI, &H6FD5C7E7UI, &H56E14EC4UI, &H362ABFCEUI, &HDDC6C837UI, _
         &HD79A3234UI, &H92638212UI, &H670EFA8EUI, &H406000E0UI}
    End Function

    Private Function SetupS3() As UInteger()
        Return New UInteger() { _
         &H3A39CE37UI, &HD3FAF5CFUI, &HABC27737UI, &H5AC52D1BUI, &H5CB0679EUI, &H4FA33742UI, _
         &HD3822740UI, &H99BC9BBEUI, &HD5118E9DUI, &HBF0F7315UI, &HD62D1C7EUI, &HC700C47BUI, _
         &HB78C1B6BUI, &H21A19045UI, &HB26EB1BEUI, &H6A366EB4UI, &H5748AB2FUI, &HBC946E79UI, _
         &HC6A376D2UI, &H6549C2C8UI, &H530FF8EEUI, &H468DDE7DUI, &HD5730A1DUI, &H4CD04DC6UI, _
         &H2939BBDBUI, &HA9BA4650UI, &HAC9526E8UI, &HBE5EE304UI, &HA1FAD5F0UI, &H6A2D519AUI, _
         &H63EF8CE2UI, &H9A86EE22UI, &HC089C2B8UI, &H43242EF6UI, &HA51E03AAUI, &H9CF2D0A4UI, _
         &H83C061BAUI, &H9BE96A4DUI, &H8FE51550UI, &HBA645BD6UI, &H2826A2F9UI, &HA73A3AE1UI, _
         &H4BA99586UI, &HEF5562E9UI, &HC72FEFD3UI, &HF752F7DAUI, &H3F046F69UI, &H77FA0A59UI, _
         &H80E4A915UI, &H87B08601UI, &H9B09E6ADUI, &H3B3EE593UI, &HE990FD5AUI, &H9E34D797UI, _
         &H2CF0B7D9UI, &H22B8B51UI, &H96D5AC3AUI, &H17DA67DUI, &HD1CF3ED6UI, &H7C7D2D28UI, _
         &H1F9F25CFUI, &HADF2B89BUI, &H5AD6B472UI, &H5A88F54CUI, &HE029AC71UI, &HE019A5E6UI, _
         &H47B0ACFDUI, &HED93FA9BUI, &HE8D3C48DUI, &H283B57CCUI, &HF8D56629UI, &H79132E28UI, _
         &H785F0191UI, &HED756055UI, &HF7960E44UI, &HE3D35E8CUI, &H15056DD4UI, &H88F46DBAUI, _
         &H3A16125UI, &H564F0BDUI, &HC3EB9E15UI, &H3C9057A2UI, &H97271AECUI, &HA93A072AUI, _
         &H1B3F6D9BUI, &H1E6321F5UI, &HF59C66FBUI, &H26DCF319UI, &H7533D928UI, &HB155FDF5UI, _
         &H3563482UI, &H8ABA3CBBUI, &H28517711UI, &HC20AD9F8UI, &HABCC5167UI, &HCCAD925FUI, _
         &H4DE81751UI, &H3830DC8EUI, &H379D5862UI, &H9320F991UI, &HEA7A90C2UI, &HFB3E7BCEUI, _
         &H5121CE64UI, &H774FBE32UI, &HA8B6E37EUI, &HC3293D46UI, &H48DE5369UI, &H6413E680UI, _
         &HA2AE0810UI, &HDD6DB224UI, &H69852DFDUI, &H9072166UI, &HB39A460AUI, &H6445C0DDUI, _
         &H586CDECFUI, &H1C20C8AEUI, &H5BBEF7DDUI, &H1B588D40UI, &HCCD2017FUI, &H6BB4E3BBUI, _
         &HDDA26A7EUI, &H3A59FF45UI, &H3E350A44UI, &HBCB4CDD5UI, &H72EACEA8UI, &HFA6484BBUI, _
         &H8D6612AEUI, &HBF3C6F47UI, &HD29BE463UI, &H542F5D9EUI, &HAEC2771BUI, &HF64E6370UI, _
         &H740E0D8DUI, &HE75B1357UI, &HF8721671UI, &HAF537D5DUI, &H4040CB08UI, &H4EB4E2CCUI, _
         &H34D2466AUI, &H115AF84UI, &HE1B00428UI, &H95983A1DUI, &H6B89FB4UI, &HCE6EA048UI, _
         &H6F3F3B82UI, &H3520AB82UI, &H11A1D4BUI, &H277227F8UI, &H611560B1UI, &HE7933FDCUI, _
         &HBB3A792BUI, &H344525BDUI, &HA08839E1UI, &H51CE794BUI, &H2F32C9B7UI, &HA01FBAC9UI, _
         &HE01CC87EUI, &HBCC7D1F6UI, &HCF0111C3UI, &HA1E8AAC7UI, &H1A908749UI, &HD44FBD9AUI, _
         &HD0DADECBUI, &HD50ADA38UI, &H339C32AUI, &HC6913667UI, &H8DF9317CUI, &HE0B12B4FUI, _
         &HF79E59B7UI, &H43F5BB3AUI, &HF2D519FFUI, &H27D9459CUI, &HBF97222CUI, &H15E6FC2AUI, _
         &HF91FC71UI, &H9B941525UI, &HFAE59361UI, &HCEB69CEBUI, &HC2A86459UI, &H12BAA8D1UI, _
         &HB6C1075EUI, &HE3056A0CUI, &H10D25065UI, &HCB03A442UI, &HE0EC6E0EUI, &H1698DB3BUI, _
         &H4C98A0BEUI, &H3278E964UI, &H9F1F9532UI, &HE0D392DFUI, &HD3A0342BUI, &H8971F21EUI, _
         &H1B0A7441UI, &H4BA3348CUI, &HC5BE7120UI, &HC37632D8UI, &HDF359F8DUI, &H9B992F2EUI, _
         &HE60B6F47UI, &HFE3F11DUI, &HE54CDA54UI, &H1EDAD891UI, &HCE6279CFUI, &HCD3E7E6FUI, _
         &H1618B166UI, &HFD2C1D05UI, &H848FD2C5UI, &HF6FB2299UI, &HF523F357UI, &HA6327623UI, _
         &H93A83531UI, &H56CCCD02UI, &HACF08162UI, &H5A75EBB5UI, &H6E163697UI, &H88D273CCUI, _
         &HDE966292UI, &H81B949D0UI, &H4C50901BUI, &H71C65614UI, &HE6C6C7BDUI, &H327A140AUI, _
         &H45E1D006UI, &HC3F27B9AUI, &HC9AA53FDUI, &H62A80F00UI, &HBB25BFE2UI, &H35BDD2F6UI, _
         &H71126905UI, &HB2040222UI, &HB6CBCF7CUI, &HCD769C2BUI, &H53113EC0UI, &H1640E3D3UI, _
         &H38ABBD60UI, &H2547ADF0UI, &HBA38209CUI, &HF746CE76UI, &H77AFA1C5UI, &H20756060UI, _
         &H85CBFE4EUI, &H8AE88DD8UI, &H7AAAF9B0UI, &H4CF9AA7EUI, &H1948C25CUI, &H2FB8A8CUI, _
         &H1C36AE4UI, &HD6EBE1F9UI, &H90D4F869UI, &HA65CDEA0UI, &H3F09252DUI, &HC208E69FUI, _
         &HB74E6132UI, &HCE77E25BUI, &H578FDFE3UI, &H3AC372E6UI}
    End Function

#End Region

#Region "Conversions"

    'gets the first byte in a uint
    Private Function wordByte0(w As UInteger) As Byte
        Return CByte(w \ 256 \ 256 \ 256 Mod 256)
    End Function

    'gets the second byte in a uint
    Private Function wordByte1(w As UInteger) As Byte
        Return CByte(w \ 256 \ 256 Mod 256)
    End Function

    'gets the third byte in a uint
    Private Function wordByte2(w As UInteger) As Byte
        Return CByte(w \ 256 Mod 256)
    End Function

    'gets the fourth byte in a uint
    Private Function wordByte3(w As UInteger) As Byte
        Return CByte(w Mod 256)
    End Function

    'converts a byte array to a hex string
    Private Function ByteToHex(bytes As Byte()) As String
        Dim s As New StringBuilder()
        For Each b As Byte In bytes
            s.Append(b.ToString("x2"))
        Next
        Return s.ToString()
    End Function

    'converts a hex string to a byte array
    Private Function HexToByte(hex As String) As Byte()
        Dim r As Byte() = New Byte(hex.Length \ 2 - 1) {}
        For i As Integer = 0 To hex.Length - 2 Step 2
            Dim a As Byte = GetHex(hex(i))
            Dim b As Byte = GetHex(hex(i + 1))
            r(i \ 2) = CByte(a * 16 + b)
        Next
        Return r
    End Function

    'converts a single hex character to it's decimal value
    Private Function GetHex(x As Char) As Byte
        If x <= "9"c AndAlso x >= "0"c Then
            Return CByte(AscW(x) - AscW("0"c))
        ElseIf x <= "z"c AndAlso x >= "a"c Then
            Return CByte(AscW(x) - AscW("a"c) + 10)
        ElseIf x <= "Z"c AndAlso x >= "A"c Then
            Return CByte(AscW(x) - AscW("A"c) + 10)
        End If
        Return 0
    End Function
#End Region
End Class

#End Region


