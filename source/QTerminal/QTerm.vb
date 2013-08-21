Imports System.Console
Module QTerminal

#Region "License"
    '*  GNU License Agreement
    '*  ---------------------
    '*  This program is free software; you can redistribute it and/or modify
    '*  it under the terms of the GNU General Public License version 3 as
    '*  published by the Free Software Foundation.
    '*
    '*  This program is distributed in the hope that it will be useful,
    '*  but WITHOUT ANY WARRANTY; without even the implied warranty of
    '*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    '*  GNU General Public License for more details.
    '*
    '*  You should have received a copy of the GNU General Public License
    '*  along with this program; if not, write to the Free Software
    '*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA
    '*
    '*  http://www.gnu.org/licenses/gpl-3.0.txt
#End Region

#Region "Vars"
    Public ReadOnly VER As String = "0.0.0.1 BETA"          'Global version var
    Dim OS As String = Nothing
    Dim i As UInt32 = 0                                     'Global counting var
    Dim cprompt As String = Nothing                         'Your prompt (like $PS1 in bash)
    Dim DefaultFColor As ConsoleColor = Nothing             'Keeps the default Foreground color
    Dim DefaultBColor As ConsoleColor = ConsoleColor.Black  'Keeps the default Background (Edited for testing)
    Dim UseColor As Boolean = Nothing                       'Global var that determines if color should be used
    Dim EH As New ErrHandler                                'Loads the error handle class
    Dim H As New Help                                       'Loads the help doc. class
#End Region

    Shared Sub Main(ByVal Args() As String)
        H.Help()
        Dim RunWarn As Boolean = True                       'If Warning should be called on start up
        Dim RunSetUp As Boolean = True                      'If Var Setup should run on start up (Pro Tip: It should)
        Dim RunInfo As Boolean = True                       'If the copy right info and version should be displayed on start up
        Do Until i = Args.Length
            Dim StopAfterCommand As Boolean = True
            If Args(i) = "-c" or Args(i) = "--command" Then
                Command(Args(i +1), StopAfterCommand)           'Runs a command then exits  
            ElseIf Args(i) = "-h" or Args(i) = "--help" Then    'Displays Help then exits
                H.Help()
                Environment.Exit(0)
            ElseIf Args(i) = "-s" or Args(i) = "--stop" Then            'Doesnt stop after running a command with -c
                StopAfterCommand = False
            ElseIf Args(i) = "-nc" or Args(i) = "--no-color" Then       'Turns off color
                UseColor = False
            ElseIf Args(i) = "-nw" or Args(i) = "--no-warnings" Then    'Turns off warnings
                RunWarn = False
            ElseIf Args(i) = "-ns" or Args(i) = "--no-setup" Then       'Skips set up (NOT recommended)
                RunSetUp = False
            ElseIf Args(i) = "-ni" or Args(i) = "--no-information" Then 'Skips boot message
                RunInfo = False
            ElseIf Args(i) = "-v" or Args(i) = "--version" Then         'Displays version then exits
                H.Info("Version")
                Environment.Exit(0)
            Else
                WriteLine("Unknown flag '" & Args(i) & "' ignoring.")   'Ignores unknown Args
            End If
            i += 1
        Loop
        If RunSetUp = True Then
            QTermLoad()
        End If
        If RunWarn = True Then
            Warnings()
        End If
        If RunInfo = True Then
            H.Info("Boot")
        End If
        Prompt()
    End Sub

    Private Function QTermLoad() As Boolean 'Loads vars if they are undefined
        'This is where vars and other key components will be loaded
        'Along with a future .qterminal.conf file
        Console.Title = "QTerminal"
        If cprompt = Nothing Then
            cprompt = "==> "
        End If
        If DefaultFColor = Nothing Then
            DefaultFColor = Console.ForeGroundColor
        Else
            Console.ForeGroundColor = DefaultFColor
        End If
        If DefaultBColor = Nothing Then
           DefaultBColor = Console.BackGroundColor 'Keeps default
        Else
            Console.BackGroundColor = DefaultBColor
        End If
        If UseColor = Nothing Then
            UseColor = True
        End If
        If OS = Nothing Then
            OS = My.Computer.Info.OSFullName
        End If
        Return True
    End Function

    Private Function Warnings() As Boolean 'Detects issues and reports them
        'This functions reports warnings on load time
        If Console.ForeGroundColor = ConsoleColor.Black Then
            Say("Warning: Default text color is set to black.", , ConsoleColor.Yellow)
        End If
        If OS = "Unix" Then
            Say("Warning: This program maybe unstable on a Unix based OS.", , ConsoleColor.Yellow)
        End If
        Return True
    End Function

    Public Sub Prompt() 'A very simple UI
        Say(cprompt, False, ConsoleColor.Cyan)
        Dim Cmd As String = ReadLine()
        If Cmd.ToLower = "clear" or Cmd.ToLower = "cls" Then
            Clear()
        ElseIf Cmd.ToLower = "exit" or Cmd.ToLower = "quit" Then
            Environment.Exit(0)
        ElseIf Cmd = Nothing Then
            Prompt()
        Else
            Command(Cmd)
        End If
        Prompt()
    End Sub

    Private Function Command(ByVal cmd As String, Optional ExitNow As Boolean = False) As Boolean
        'Command interpetor
        'NOTE: If it gets to big it will be moved to its own class
        Try
            If cmd = "fix" Then
                If UseColor = True Then
                    Say("Fixing colors...")
                    Console.BackGroundColor = ConsoleColor.Black
                    DefaultBColor = Nothing
                    Clear()
                End If
                Say("Reloading variables...")
                QTermLoad()
            ElseIf cmd = "version" or cmd = "ver" Then
                H.Info("Version")
            ElseIf cmd = "help" Then
                H.Help()
            ElseIf cmd = "echo" Then
                Say(Nothing)
                
            ElseIf cmd.Contains(" ") = True Then 'if the command has args
                Dim inpt() As String = Split(cmd)
                i = 1
                If inpt(0) = "echo" Then
                    If inpt.Length = 0 Then
                        Say(" ")
                    Else
                        Do Until i = inpt.Length -1
                            Say(inpt(i) & " ", False)
                            i += 1
                        Loop
                        Say(inpt(i))
                    End If
                ElseIf inpt(0) = "help" Then
                    H.Help()
                Else
                    Say("Command not found.")
                End If
                If ExitNow = True Then
                    Environment.Exit(0)
                End If
            Else
                Say("Command not found.")
            End If
            Return True
        Catch
            EH.Er(Err.Number, Err.Description)
            Return False
        End Try
    End Function

    Public Function Say(ByVal Msg As String, Optional EndLine As Boolean = True, Optional TextColor As ConsoleColor = ConsoleColor.White) As Boolean ', Optional BackColor As ConsoleColor = Nothing) As Boolean
        'This function is to be called when a command needs to write to Stdin
        If UseColor = True Then
            Console.ForegroundColor = TextColor
            If TextColor <> Nothing Then
                Console.ForeGroundColor = TextColor
            Else
                TextColor = DefaultFColor
            End If
            If EndLine = True Then
                Console.WriteLine(Msg)
            ElseIf EndLine = False Then
                Console.Write(Msg)
            End If
            Console.ForegroundColor = DefaultFColor
            'Console.BackGroundColor = DefaultBColor
        Else
            If EndLine = True Then
                Console.WriteLine(Msg)
            ElseIf EndLine = False Then
                Console.Write(Msg)
            End If
        End If
        Return True
    End Function
End Module
