Imports System.IO
Imports System.Timers
Imports Newtonsoft

Module OnsetPackageCreator
    Dim WithEvents _timer As New Timer
    Dim _Author As String
    Dim _Version As String
    Dim _ServerFolder As String
    Dim _ClientFolder As String
    Dim _Package As Package
    Sub Main()
        Console.WriteLine("Start Onset Auto Package Creator...")
        Console.WriteLine("Put this exe in package folder.")
        Console.WriteLine("====================================")
        Console.WriteLine("> Enter Author Name")
        _Author = Console.ReadLine()
        Console.WriteLine("> Enter Version")
        _Version = Console.ReadLine()
        Console.WriteLine("> Enter Server script folder name")
        _ServerFolder = Console.ReadLine()
        Console.WriteLine("> Enter Client script folder name")
        _ClientFolder = Console.ReadLine()
        Console.WriteLine("> Enter Interval for Creation in MS. ( eg 1000 = 1 sec )")
        Dim Interval As String = Console.ReadLine()
        _timer.Interval = Interval
        _timer.Start()
        Console.ReadLine()
    End Sub
    Private Sub Timer_Tick() Handles _timer.Elapsed
        Dim onsetPackageJson As New Package
        onsetPackageJson.author = _Author
        onsetPackageJson.version = _Version
        Dim allServerFiles() As String = Directory.GetFiles(Directory.GetCurrentDirectory & "\" & _ServerFolder, "*.*", SearchOption.AllDirectories)
        For Each file In allServerFiles
            Dim relativeFilePath As String = file.Replace(Directory.GetCurrentDirectory & "\", "").Replace("\", "/")
            onsetPackageJson.server_scripts.Add(relativeFilePath)
        Next
        Dim allClientFiles() As String = Directory.GetFiles(Directory.GetCurrentDirectory & "\" & _ClientFolder, "*.*", SearchOption.AllDirectories)
        For Each file In allClientFiles
            Dim ext As String = Path.GetExtension(file)
            If ext = ".lua" Then
                Dim relativeFilePath As String = file.Replace(Directory.GetCurrentDirectory & "\", "").Replace("\", "/")
                onsetPackageJson.client_scripts.Add(relativeFilePath)
            Else
                Dim relativeFilePath As String = file.Replace(Directory.GetCurrentDirectory & "\", "").Replace("\", "/")
                onsetPackageJson.files.Add(relativeFilePath)
            End If
        Next
        Dim jsonStr As String = Json.JsonConvert.SerializeObject(onsetPackageJson)
        File.WriteAllText("package.json", jsonStr)
        Console.WriteLine("[" & DateTime.Now & "] Package.json created.")
    End Sub
End Module

Class Package
    Property author As String
    Property version As String
    Property server_scripts As New List(Of String)
    Property client_scripts As New List(Of String)
    Property files As New List(Of String)
End Class
