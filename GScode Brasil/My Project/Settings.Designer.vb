﻿'------------------------------------------------------------------------------
' <auto-generated>
'     O código foi gerado por uma ferramenta.
'     Versão de Tempo de Execução:4.0.30319.42000
'
'     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
'     o código for gerado novamente.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "Funcionalidade de salvamento automático do My.Settings"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.250")>  _
        Public Property Database_Servidor() As String
            Get
                Return CType(Me("Database_Servidor"),String)
            End Get
            Set
                Me("Database_Servidor") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1433")>  _
        Public Property Database_Porta() As String
            Get
                Return CType(Me("Database_Porta"),String)
            End Get
            Set
                Me("Database_Porta") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("gscode")>  _
        Public Property Database_Banco() As String
            Get
                Return CType(Me("Database_Banco"),String)
            End Get
            Set
                Me("Database_Banco") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("sa")>  _
        Public Property Database_Usuario() As String
            Get
                Return CType(Me("Database_Usuario"),String)
            End Get
            Set
                Me("Database_Usuario") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("ADcYhGwf4S2kbyMKqQRmcidLfnf9ZbrmuRs2d5tdEOromEmqty3BrkD3v/cFMU0X")>  _
        Public Property Database_Senha() As String
            Get
                Return CType(Me("Database_Senha"),String)
            End Get
            Set
                Me("Database_Senha") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("192.168.1.250")>  _
        Public Property SFTP_Servidor() As String
            Get
                Return CType(Me("SFTP_Servidor"),String)
            End Get
            Set
                Me("SFTP_Servidor") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("22")>  _
        Public Property SFTP_Porta() As String
            Get
                Return CType(Me("SFTP_Porta"),String)
            End Get
            Set
                Me("SFTP_Porta") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("mssql")>  _
        Public Property SFTP_Usuario() As String
            Get
                Return CType(Me("SFTP_Usuario"),String)
            End Get
            Set
                Me("SFTP_Usuario") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("gXNbIr2dNP8YnGTZQnCCZfxlIlwq23+7xQp1dn0JP6M=")>  _
        Public Property SFTP_Senha() As String
            Get
                Return CType(Me("SFTP_Senha"),String)
            End Get
            Set
                Me("SFTP_Senha") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SFTP_ChaveSSH() As String
            Get
                Return CType(Me("SFTP_ChaveSSH"),String)
            End Get
            Set
                Me("SFTP_ChaveSSH") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/sql/backup/")>  _
        Public Property SFTP_BackupDatabase() As String
            Get
                Return CType(Me("SFTP_BackupDatabase"),String)
            End Get
            Set
                Me("SFTP_BackupDatabase") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/GSHome/Backup_Sistema/")>  _
        Public Property SFTP_BackupSistema() As String
            Get
                Return CType(Me("SFTP_BackupSistema"),String)
            End Get
            Set
                Me("SFTP_BackupSistema") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Phonebook/Clientes/")>  _
        Public Property SFTP_PhonebookClientes() As String
            Get
                Return CType(Me("SFTP_PhonebookClientes"),String)
            End Get
            Set
                Me("SFTP_PhonebookClientes") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Phonebook/Colaboradores/")>  _
        Public Property SFTP_PhonebookColaborador() As String
            Get
                Return CType(Me("SFTP_PhonebookColaborador"),String)
            End Get
            Set
                Me("SFTP_PhonebookColaborador") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Phonebook/Fornecedores/")>  _
        Public Property SFTP_PhonebookFornecedor() As String
            Get
                Return CType(Me("SFTP_PhonebookFornecedor"),String)
            End Get
            Set
                Me("SFTP_PhonebookFornecedor") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Phonebook/Parceiros/")>  _
        Public Property SFTP_PhonebookParceiro() As String
            Get
                Return CType(Me("SFTP_PhonebookParceiro"),String)
            End Get
            Set
                Me("SFTP_PhonebookParceiro") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Helpdesk/OS Atendimento Externo/")>  _
        Public Property SFTP_HelpdeskOSExterno() As String
            Get
                Return CType(Me("SFTP_HelpdeskOSExterno"),String)
            End Get
            Set
                Me("SFTP_HelpdeskOSExterno") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Helpdesk/OS Atendimento Remoto/")>  _
        Public Property SFTP_HelpdeskOSRemoto() As String
            Get
                Return CType(Me("SFTP_HelpdeskOSRemoto"),String)
            End Get
            Set
                Me("SFTP_HelpdeskOSRemoto") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Estoque/")>  _
        Public Property SFTP_Estoque() As String
            Get
                Return CType(Me("SFTP_Estoque"),String)
            End Get
            Set
                Me("SFTP_Estoque") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Financeiro/NFe/")>  _
        Public Property SFTP_Financeiro_Nfe() As String
            Get
                Return CType(Me("SFTP_Financeiro_Nfe"),String)
            End Get
            Set
                Me("SFTP_Financeiro_Nfe") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Financeiro/NFSe/")>  _
        Public Property SFTP_Financeiro_NFSe() As String
            Get
                Return CType(Me("SFTP_Financeiro_NFSe"),String)
            End Get
            Set
                Me("SFTP_Financeiro_NFSe") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Dashboard/Phonebook/")>  _
        Public Property SFTP_Dashboard_Phonebook() As String
            Get
                Return CType(Me("SFTP_Dashboard_Phonebook"),String)
            End Get
            Set
                Me("SFTP_Dashboard_Phonebook") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Dashboard/Helpdesk/")>  _
        Public Property SFTP_Dashboard_Helpdesk() As String
            Get
                Return CType(Me("SFTP_Dashboard_Helpdesk"),String)
            End Get
            Set
                Me("SFTP_Dashboard_Helpdesk") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Dashboard/Estoque/")>  _
        Public Property SFTP_Dashboard_Estoque() As String
            Get
                Return CType(Me("SFTP_Dashboard_Estoque"),String)
            End Get
            Set
                Me("SFTP_Dashboard_Estoque") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/Dashboard/Financeiro/")>  _
        Public Property SFTP_Dashboard_Financeiro() As String
            Get
                Return CType(Me("SFTP_Dashboard_Financeiro"),String)
            End Get
            Set
                Me("SFTP_Dashboard_Financeiro") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("mail.erimat.com.br")>  _
        Public Property SMTP_Servidor() As String
            Get
                Return CType(Me("SMTP_Servidor"),String)
            End Get
            Set
                Me("SMTP_Servidor") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("587")>  _
        Public Property SMTP_Porta() As String
            Get
                Return CType(Me("SMTP_Porta"),String)
            End Get
            Set
                Me("SMTP_Porta") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("suporte@erimat.com.br")>  _
        Public Property SMTP_Usuario() As String
            Get
                Return CType(Me("SMTP_Usuario"),String)
            End Get
            Set
                Me("SMTP_Usuario") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("gXNbIr2dNP8YnGTZQnCCZfxlIlwq23+7xQp1dn0JP6M=")>  _
        Public Property SMTP_Senha() As String
            Get
                Return CType(Me("SMTP_Senha"),String)
            End Get
            Set
                Me("SMTP_Senha") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("guilherme@erimat.com.br")>  _
        Public Property SMTP_Administrador() As String
            Get
                Return CType(Me("SMTP_Administrador"),String)
            End Get
            Set
                Me("SMTP_Administrador") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property SMTP_SSL() As Boolean
            Get
                Return CType(Me("SMTP_SSL"),Boolean)
            End Get
            Set
                Me("SMTP_SSL") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("admin@gscodebrasil.com.br")>  _
        Public Property AcessoAdmin_Usuario() As String
            Get
                Return CType(Me("AcessoAdmin_Usuario"),String)
            End Get
            Set
                Me("AcessoAdmin_Usuario") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("gXNbIr2dNP8YnGTZQnCCZfxlIlwq23+7xQp1dn0JP6M=")>  _
        Public Property AcessoAdmin_Senha() As String
            Get
                Return CType(Me("AcessoAdmin_Senha"),String)
            End Get
            Set
                Me("AcessoAdmin_Senha") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property AutoLogin() As Boolean
            Get
                Return CType(Me("AutoLogin"),Boolean)
            End Get
            Set
                Me("AutoLogin") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property LembrarAcesso() As Boolean
            Get
                Return CType(Me("LembrarAcesso"),Boolean)
            End Get
            Set
                Me("LembrarAcesso") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property Acesso_Usuario() As String
            Get
                Return CType(Me("Acesso_Usuario"),String)
            End Get
            Set
                Me("Acesso_Usuario") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property Acesso_Senha() As String
            Get
                Return CType(Me("Acesso_Senha"),String)
            End Get
            Set
                Me("Acesso_Senha") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property Softphone_Nome() As String
            Get
                Return CType(Me("Softphone_Nome"),String)
            End Get
            Set
                Me("Softphone_Nome") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property Softphone_Local() As String
            Get
                Return CType(Me("Softphone_Local"),String)
            End Get
            Set
                Me("Softphone_Local") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("/gscode/GSHome/Usuarios/")>  _
        Public Property SFTP_ImagemUsuario() As String
            Get
                Return CType(Me("SFTP_ImagemUsuario"),String)
            End Get
            Set
                Me("SFTP_ImagemUsuario") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property Softphone_Prefixo() As String
            Get
                Return CType(Me("Softphone_Prefixo"),String)
            End Get
            Set
                Me("Softphone_Prefixo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property Softphone_AceptPrefixo() As Boolean
            Get
                Return CType(Me("Softphone_AceptPrefixo"),Boolean)
            End Get
            Set
                Me("Softphone_AceptPrefixo") = value
            End Set
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.GScode_Brasil.My.MySettings
            Get
                Return Global.GScode_Brasil.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
