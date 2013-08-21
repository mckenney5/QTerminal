'#####################################
'# All Error Related Stuff Goes Here #
'#####################################

Option Explicit On
Option Strict On
Imports System
Public Class ErrHandler
	
Public Function Er(ByVal ErNum As Integer, ByVal ErMsg As String) As Boolean
        'Handles Errors and allows you to create your own
		Say("Error (", False, ConsoleColor.Red)
        If ErNum = Nothing Then
            ErNum = 0
        End If
		If ErNum = 0 Then
			Say("0) the error handler was called but an error did not occur.", , ConsoleColor.Red)
            Say("You dun' goofed programmer")
		Else
			Say(ErNum & ") " & ErMsg, , ConsoleColor.Red) 'If Error is unhandled then it will handle it here
			Say("This error was unhandled. Please report it.", , ConsoleColor.Yellow)
		End If
		Return True
	End Function
End Class

