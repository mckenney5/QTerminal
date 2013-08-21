'####################################
'# All Help Related Stuff Goes Here #
'####################################

Option Explicit On
Option Strict On
Imports System
Imports System.Console
Public Class Help
 
    Public Function Help() As Boolean
        'Lists all help information (uses WriteLine instead of Say)
        WriteLine("QTerminal Version: " & VER)
        Info("Boot")
        WriteLine(VbNewLine & "Usage: qterminal [flag]")
        WriteLine("-c, --command            Runs a command then exits")
        WriteLine("-h, --help               Displays this then exits")
        WriteLine("-nc, --no-color          Turns off color")
        WriteLine("-ni, --no-information    Does not display boot message")
        WriteLine("-ns, --no-setup          Skips variables set up (not recommended)")
        WriteLine("-nw, --no-warning        Skips detecting of warnings on launch")
        WriteLine("-v, --version            Displays current version then exits")
        WriteLine(VbNewLine & "For a list of commands type 'help commands'")
        Return True
    End Function
    
    Public Function Info(ByVal Type As String) As Boolean 'Displays information
        If Type = "Boot" Then
            Say("Welcome to QTerminal, a free (as in freedom) shell for Windows and *nix operating systems." & VbNewLine & "Type 'help commands' for help", , ConsoleColor.White)
        ElseIf Type = "Commands" Then 'Should be in ABC order
            Say("Clear              Clears the screen")
            Say("Cls                Clears the screen")
            Say("Echo [text]        Prints text to the screen")
            Say("Exit               Exits QTerminal")
            Say("Fix                Fixes colors and reloads variables")
            Say("Help [term]        Displays help documentation")
            Say("Quit               Exits QTerminal")
            Say("Version            Displays current version")
        ElseIf Type = "Version" Then
            Say("QTerminal, version " & VER)
            Say("Copyright (C) 2013 Quite Tiny Software")
            Say("License GPLv3+: GNU GPL version 3 or later <http://gnu.org/licenses/gpl.html>" & VbNewLine)
            Say("This is free as in freedom software; you are free to change and redistribute it.")
            Say("There is NO WARRANTY, to the extent permitted by law.")
        End If
        Return True
    End Function
End Class

