﻿Imports System.ComponentModel
Imports System.IO
Imports Renci.SshNet
Imports Guna.UI2.WinForms
Imports Bunifu.UI.WinForms

Public Class ServerSFTP

    Private ReadOnly ClCifer As New Cifer

#Region "Geral"

    Public Sub Connect()
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                MessageBox.Show($"Conexão estabelecida com sucesso.", "INFORMAÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível conectar-se ao servidor, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Private Function Getconnection() As SftpClient
        Dim Sftp As New SftpClient(My.Settings.SFTP_Servidor, My.Settings.SFTP_Porta, My.Settings.SFTP_Usuario, ClCifer.Decriptar(My.Settings.SFTP_Senha, ClCifer.senha))
        Return Sftp
    End Function

#End Region

#Region "Backup"

    Private Function FormataGridView(ColumnChkBox As Boolean, ColumnName As String, ColumnData As String, ColumnTamanho As String, Dgv As BunifuDataGridView)
        With Dgv

            .GridColor = Color.WhiteSmoke
            .DefaultCellStyle.Font = New Font("Segoe UI Semibold", 9)
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(223, 243, 251)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.White
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .RowHeadersDefaultCellStyle.Font = New Font("Segoe UI Bold", 9)
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersHeight = 40
            .RowTemplate.Height = 40

            .ColumnCount = 5

            .Rows.Add(ColumnChkBox, ColumnName, ColumnData, ColumnTamanho)

            .Columns(0).Visible = True
            .Columns(0).HeaderCell = New CheckHeaderCell
            .Columns(1).HeaderText = "Arquivo"
            .Columns(2).HeaderText = "Data"
            .Columns(3).HeaderText = "Tamanho"
            .Sort(.Columns(2), ListSortDirection.Descending)
            .Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With
        Return Dgv
    End Function

    Public Function FileList(Dgv As BunifuDataGridView)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                Dim Files = Sftp.ListDirectory($"{My.Settings.SFTP_BackupDatabase}")

                For Each File In Files
                    If File.IsRegularFile Then
                        Dim FileAtributes = Sftp.GetAttributes($"{My.Settings.SFTP_BackupDatabase}{File.Name}")
                        Dim Tamanho As String = FileAtributes.Size
                        Dim MB = 1048576
                        Dim GB = 1048576000

                        If Tamanho < GB Then
                            Tamanho /= MB
                            Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} MB"

                        Else
                            Tamanho /= GB
                            Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} GB"

                        End If

                        FormataGridView(False, File.Name, FileAtributes.LastWriteTime, Tamanho, Dgv)

                    End If
                Next
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível listar os backups do banco de dados, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Sub FazerBackup_Sistema(Email As String, TabControl As Guna2TabControl, Timer As Timer)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                Dim dir = Sftp.Exists($"{My.Settings.SFTP_BackupSistema}{Email}")

                Select Case dir
                    Case False
                        Sftp.CreateDirectory($"{My.Settings.SFTP_BackupSistema}{Email}")

                        Using stream As Stream = File.OpenRead($"{Application.StartupPath}\GScode Brasil.exe.config")
                            Sftp.UploadFile(stream, $"{My.Settings.SFTP_BackupSistema}{Email}/GScode Brasil.exe.config")
                        End Using

                    Case Else
                        Using stream As Stream = File.OpenRead($"{Application.StartupPath}\GScode Brasil.exe.config")
                            Sftp.UploadFile(stream, $"{My.Settings.SFTP_BackupSistema}{Email}/GScode Brasil.exe.config")
                        End Using

                End Select

            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível salvar o backup do sistema, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Sub RestaurarBackup_Sistema(TabControl As Guna2TabControl, Timer As Timer)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                Dim dir = Sftp.Exists($"{My.Settings.SFTP_BackupSistema}{FrmPrincipal.LblEmailLeft.Text}")
                Dim files = Sftp.Exists($"{My.Settings.SFTP_BackupSistema}{FrmPrincipal.LblEmailLeft.Text}/{My.Settings.SFTP_BackupSistema}")

                Select Case dir
                    Case False
                        Timer.Stop()
                        TabControl.SelectTab(0)
                        MessageBox.Show($"O usuário não possui nenhum backup, por favor realize um backup do seu sistema.", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                    Case Else
                        If files = False Then
                            Timer.Stop()
                            TabControl.SelectTab(0)
                            MessageBox.Show($"O usuário não possui nenhum backup, por favor realize um backup do seu sistema.", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                        Else
                            Using stream As Stream = File.OpenWrite($"{Application.StartupPath}\{My.Settings.SFTP_BackupSistema}")
                                Sftp.DownloadFile($"{My.Settings.SFTP_BackupSistema}{FrmPrincipal.LblEmailLeft.Text}/{My.Settings.SFTP_BackupSistema}", stream)
                            End Using

                        End If
                End Select

            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível restaurar o backup do sistema, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Function CarregarBackup(Dgv As BunifuDataGridView, Ofd As OpenFileDialog, TabControl As Guna2TabControl)
        Ofd.Title = "Selecione Arquivo"
        Ofd.Filter = "Arquivos (*.bak)|*.bak"
        Ofd.Multiselect = True
        Ofd.CheckFileExists = True
        Ofd.CheckPathExists = True
        Ofd.FilterIndex = 2
        Ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Ofd.ReadOnlyChecked = True
        Ofd.ShowReadOnly = True
        Ofd.FileName = ""

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim FilePath() As String = Ofd.FileNames
                    Dim FileName As String

                    'VERIFICA AS INFORMAÇÕES (ULTIMA ATUALIZAÇÃO E TAMANHO) DO ARQUIVO
                    For Each FileName In FilePath
                        Dim FileWriteTime As String = File.GetLastWriteTime(FileName)
                        Dim FileSize As String = File.OpenRead(FileName).Length
                        Dim MB = 1048576
                        Dim GB = 1048576000

                        If FileSize < GB Then
                            FileSize /= MB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} MB"

                        Else
                            FileSize /= GB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} GB"

                        End If

                        'VERIFICA SE EXISTE O ARQUIVO NO SERVIDOR
                        If Sftp.Exists($"{My.Settings.SFTP_BackupDatabase}{Path.GetFileName(FileName)}") Then
                            If MessageBox.Show($"O Arquivo {Path.GetFileName(FileName)} já existe no servidor, deseja substitui-lo?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then

                                'REMOVE A LINHA CORRESPONDENTE AO ARQUIVO QUE FOI SUBSTITUIDO
                                For Each VFile As DataGridViewRow In Dgv.Rows
                                    If VFile.Cells(1).Value.ToString.Contains(Path.GetFileName(FileName)) Then
                                        Dgv.Rows.Remove(VFile)
                                    End If
                                Next

                                'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                                Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                    Sftp.UploadFile(stream, $"{My.Settings.SFTP_BackupDatabase}{Path.GetFileName(FileName)}")

                                    If Dgv.Rows.Count > 0 Then
                                        Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                    Else
                                        FormataGridView(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                    End If
                                End Using
                            End If

                        Else

                            'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                            Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                Sftp.UploadFile(stream, $"{My.Settings.SFTP_BackupDatabase}{Path.GetFileName(FileName)}")

                                If Dgv.Rows.Count > 0 Then
                                    Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                Else
                                    FormataGridView(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                End If
                            End Using
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível carregar o arquivo para o servidor, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Sub FileDownload(Dgv As BunifuDataGridView, Fbd As FolderBrowserDialog, Inf As String, Progress As Guna2ProgressBar, TabControl As Guna2TabControl, Timer As Timer)
        Fbd.Description = "Selecione uma pasta para realizar o Download"
        Fbd.RootFolder = Environment.SpecialFolder.MyComputer
        Fbd.ShowNewFolderButton = True

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Fbd.ShowDialog = DialogResult.OK Then
                    Inf = $"<b>Aguarde um momento, estamos processando sua solicitação...</b>"
                    Progress.Value = 0
                    TabControl.SelectTab(3)
                    Timer.Start()

                    For Each row As DataGridViewRow In Dgv.Rows()
                        If Convert.ToBoolean(row.Cells(0).Value) Then
                            Dim Files As String = row.Cells(1).Value
                            If Sftp.Exists($"{My.Settings.SFTP_BackupDatabase}{Path.GetFileName(Files)}") Then
                                Using stream As Stream = File.OpenWrite($"{Fbd.SelectedPath}\{Files}")
                                    Sftp.DownloadFile($"{My.Settings.SFTP_BackupDatabase}{Path.GetFileName(Files)}", stream)
                                End Using
                            End If
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível fazer o download do arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Function FileDelete(Dgv As BunifuDataGridView, Panel As Guna2ShadowPanel)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                Dim i As Integer = 0
                While i < Dgv.Rows.Count
                    Dim row As DataGridViewRow = Dgv.Rows(i)
                    If Convert.ToBoolean(row.Cells(0).Value) Then
                        Dim Files As String = row.Cells(1).Value
                        Sftp.DeleteFile($"{My.Settings.SFTP_BackupDatabase}{Files}")
                        Dgv.Rows.Remove(row)
                        Panel.Visible = False
                        i -= 1
                    End If
                    i += 1
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível excluir o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

#End Region

#Region "Anexo de cliente - Phonebook"

    Public Function FormataGridView_Phonebook(ColumnChkBox As Boolean, ColumnName As String, ColumnData As String, ColumnTamanho As String, Dgv As BunifuDataGridView)
        With Dgv

            .GridColor = Color.WhiteSmoke
            .DefaultCellStyle.Font = New Font("Segoe UI Semibold", 9)
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(223, 243, 251)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.White
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .RowHeadersDefaultCellStyle.Font = New Font("Segoe UI Bold", 9)
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersHeight = 40
            .RowTemplate.Height = 40

            .ColumnCount = 5

            .Rows.Add(ColumnChkBox, ColumnName, ColumnData, ColumnTamanho)

            .Columns(1).HeaderText = "Arquivo"
            .Columns(2).HeaderText = "Data"
            .Columns(3).HeaderText = "Tamanho"
            .Columns(0).HeaderCell = New CheckHeaderCell

            .Columns(0).Visible = True
            .Columns(0).ReadOnly = False

            .Sort(.Columns(2), ListSortDirection.Descending)
            .Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With
        Return Dgv
    End Function

    Public Function CarregarArquivoCliente_Phonebook(Dgv As BunifuDataGridView, Ofd As OpenFileDialog, Cliente As String, TabControl As Guna2TabControl)
        Ofd.Title = "Selecione Arquivo"
        Ofd.Filter = "Todos os arquivos (*.*)|*.*"
        Ofd.Multiselect = True
        Ofd.CheckFileExists = True
        Ofd.CheckPathExists = True
        Ofd.FilterIndex = 2
        Ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Ofd.ReadOnlyChecked = True
        Ofd.ShowReadOnly = True
        Ofd.FileName = ""

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim FilePath() As String = Ofd.FileNames
                    Dim FileName As String

                    'VERIFICA AS INFORMAÇÕES (ULTIMA ATUALIZAÇÃO E TAMANHO) DO ARQUIVO
                    For Each FileName In FilePath
                        Dim FileWriteTime As String = File.GetLastWriteTime(FileName)
                        Dim FileSize As String = File.OpenRead(FileName).Length
                        Dim MB = 1048576
                        Dim GB = 1048576000

                        If FileSize < GB Then
                            FileSize /= MB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} MB"

                        Else
                            FileSize /= GB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} GB"
                        End If

                        If Not Sftp.Exists($"{My.Settings.SFTP_PhonebookClientes}{Cliente}") Then
                            Sftp.CreateDirectory($"{My.Settings.SFTP_PhonebookClientes}{Cliente}")
                        End If

                        'VERIFICA SE EXISTE O ARQUIVO NO SERVIDOR
                        If Sftp.Exists($"{My.Settings.SFTP_PhonebookClientes}{Cliente}/{Path.GetFileName(FileName)}") Then
                            If MessageBox.Show($"O Arquivo {Path.GetFileName(FileName)} já existe no servidor, deseja substitui-lo?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then

                                'REMOVE A LINHA CORRESPONDENTE AO ARQUIVO QUE FOI SUBSTITUIDO
                                For Each VFile As DataGridViewRow In Dgv.Rows
                                    If VFile.Cells(1).Value.ToString.Contains(Path.GetFileName(FileName)) Then
                                        Dgv.Rows.Remove(VFile)
                                    End If
                                Next

                                'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                                Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                    Sftp.UploadFile(stream, $"{My.Settings.SFTP_PhonebookClientes}{Cliente}/{Path.GetFileName(FileName)}")

                                    If Dgv.Rows.Count > 0 Then
                                        Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                    Else
                                        FormataGridView_Phonebook(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                    End If
                                End Using
                            End If

                        Else

                            'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                            Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                Sftp.UploadFile(stream, $"{My.Settings.SFTP_PhonebookClientes}{Cliente}/{Path.GetFileName(FileName)}")

                                If Dgv.Rows.Count > 0 Then
                                    Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                Else
                                    FormataGridView_Phonebook(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                End If
                            End Using
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível carregar o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Function CarregarArquivoColaborador_Phonebook(Dgv As BunifuDataGridView, Ofd As OpenFileDialog, Colaborador As String, TabControl As Guna2TabControl)
        Ofd.Title = "Selecione Arquivo"
        Ofd.Filter = "Todos os arquivos (*.*)|*.*"
        Ofd.Multiselect = True
        Ofd.CheckFileExists = True
        Ofd.CheckPathExists = True
        Ofd.FilterIndex = 2
        Ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Ofd.ReadOnlyChecked = True
        Ofd.ShowReadOnly = True
        Ofd.FileName = ""

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim FilePath() As String = Ofd.FileNames
                    Dim FileName As String

                    'VERIFICA AS INFORMAÇÕES (ULTIMA ATUALIZAÇÃO E TAMANHO) DO ARQUIVO
                    For Each FileName In FilePath
                        Dim FileWriteTime As String = File.GetLastWriteTime(FileName)
                        Dim FileSize As String = File.OpenRead(FileName).Length
                        Dim MB = 1048576
                        Dim GB = 1048576000

                        If FileSize < GB Then
                            FileSize /= MB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} MB"

                        Else
                            FileSize /= GB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} GB"
                        End If

                        If Not Sftp.Exists($"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}") Then
                            Sftp.CreateDirectory($"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}")
                        End If

                        'VERIFICA SE EXISTE O ARQUIVO NO SERVIDOR
                        If Sftp.Exists($"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}/{Path.GetFileName(FileName)}") Then
                            If MessageBox.Show($"O Arquivo {Path.GetFileName(FileName)} já existe no servidor, deseja substitui-lo?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then

                                'REMOVE A LINHA CORRESPONDENTE AO ARQUIVO QUE FOI SUBSTITUIDO
                                For Each VFile As DataGridViewRow In Dgv.Rows
                                    If VFile.Cells(1).Value.ToString.Contains(Path.GetFileName(FileName)) Then
                                        Dgv.Rows.Remove(VFile)
                                    End If
                                Next

                                'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                                Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                    Sftp.UploadFile(stream, $"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}/{Path.GetFileName(FileName)}")

                                    If Dgv.Rows.Count > 0 Then
                                        Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                    Else
                                        FormataGridView_Phonebook(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                    End If
                                End Using
                            End If

                        Else

                            'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                            Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                Sftp.UploadFile(stream, $"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}/{Path.GetFileName(FileName)}")

                                If Dgv.Rows.Count > 0 Then
                                    Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                Else
                                    FormataGridView_Phonebook(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                End If
                            End Using
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível carregar o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Function CarregarArquivoFornecedor_Phonebook(Dgv As BunifuDataGridView, Ofd As OpenFileDialog, Fornecedor As String, TabControl As Guna2TabControl)
        Ofd.Title = "Selecione Arquivo"
        Ofd.Filter = "Todos os arquivos (*.*)|*.*"
        Ofd.Multiselect = True
        Ofd.CheckFileExists = True
        Ofd.CheckPathExists = True
        Ofd.FilterIndex = 2
        Ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Ofd.ReadOnlyChecked = True
        Ofd.ShowReadOnly = True
        Ofd.FileName = ""

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim FilePath() As String = Ofd.FileNames
                    Dim FileName As String

                    'VERIFICA AS INFORMAÇÕES (ULTIMA ATUALIZAÇÃO E TAMANHO) DO ARQUIVO
                    For Each FileName In FilePath
                        Dim FileWriteTime As String = File.GetLastWriteTime(FileName)
                        Dim FileSize As String = File.OpenRead(FileName).Length
                        Dim MB = 1048576
                        Dim GB = 1048576000

                        If FileSize < GB Then
                            FileSize /= MB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} MB"

                        Else
                            FileSize /= GB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} GB"
                        End If

                        If Not Sftp.Exists($"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}") Then
                            Sftp.CreateDirectory($"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}")
                        End If

                        'VERIFICA SE EXISTE O ARQUIVO NO SERVIDOR
                        If Sftp.Exists($"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}/{Path.GetFileName(FileName)}") Then
                            If MessageBox.Show($"O Arquivo {Path.GetFileName(FileName)} já existe no servidor, deseja substitui-lo?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then

                                'REMOVE A LINHA CORRESPONDENTE AO ARQUIVO QUE FOI SUBSTITUIDO
                                For Each VFile As DataGridViewRow In Dgv.Rows
                                    If VFile.Cells(1).Value.ToString.Contains(Path.GetFileName(FileName)) Then
                                        Dgv.Rows.Remove(VFile)
                                    End If
                                Next

                                'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                                Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                    Sftp.UploadFile(stream, $"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}/{Path.GetFileName(FileName)}")

                                    If Dgv.Rows.Count > 0 Then
                                        Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                    Else
                                        FormataGridView_Phonebook(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                    End If
                                End Using
                            End If

                        Else

                            'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                            Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                Sftp.UploadFile(stream, $"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}/{Path.GetFileName(FileName)}")

                                If Dgv.Rows.Count > 0 Then
                                    Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                Else
                                    FormataGridView_Phonebook(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                End If
                            End Using
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível carregar o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Function FileListCliente_Phonebook(Dgv As BunifuDataGridView, Cliente As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Sftp.Exists($"{My.Settings.SFTP_PhonebookClientes}{Cliente}/") Then
                    Dim Files = Sftp.ListDirectory($"{My.Settings.SFTP_PhonebookClientes}{Cliente}/")

                    For Each File In Files
                        If File.IsRegularFile Then
                            Dim FileAtributes = Sftp.GetAttributes($"{My.Settings.SFTP_PhonebookClientes}{Cliente}/{File.Name}")
                            Dim Tamanho As String = FileAtributes.Size
                            Dim MB = 1048576
                            Dim GB = 1048576000

                            If Tamanho < GB Then
                                Tamanho /= MB
                                Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} MB"

                            Else
                                Tamanho /= GB
                                Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} GB"

                            End If

                            FormataGridView_Phonebook(False, File.Name, FileAtributes.LastWriteTime, Tamanho, Dgv)

                        End If
                    Next
                Else
                    FormataGridView_Phonebook(False, Nothing, Nothing, Nothing, Dgv)
                    Dgv.Rows.Clear()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível listar os arquivos, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Function FileListColaborador_Phonebook(Dgv As BunifuDataGridView, Colaborador As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Sftp.Exists($"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}/") Then
                    Dim Files = Sftp.ListDirectory($"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}/")

                    For Each File In Files
                        If File.IsRegularFile Then
                            Dim FileAtributes = Sftp.GetAttributes($"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}/{File.Name}")
                            Dim Tamanho As String = FileAtributes.Size
                            Dim MB = 1048576
                            Dim GB = 1048576000

                            If Tamanho < GB Then
                                Tamanho /= MB
                                Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} MB"

                            Else
                                Tamanho /= GB
                                Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} GB"

                            End If

                            FormataGridView_Phonebook(False, File.Name, FileAtributes.LastWriteTime, Tamanho, Dgv)

                        End If
                    Next
                Else
                    FormataGridView_Phonebook(False, Nothing, Nothing, Nothing, Dgv)
                    Dgv.Rows.Clear()
                End If

            End Using
        Catch ex As Exception
            MessageBox.Show($"Erro ao listar os arquivos.{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return Dgv
    End Function

    Public Function FileListFornecedor_Phonebook(Dgv As BunifuDataGridView, Fornecedor As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Sftp.Exists($"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}/") Then
                    Dim Files = Sftp.ListDirectory($"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}/")

                    For Each File In Files
                        If File.IsRegularFile Then
                            Dim FileAtributes = Sftp.GetAttributes($"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}/{File.Name}")
                            Dim Tamanho As String = FileAtributes.Size
                            Dim MB = 1048576
                            Dim GB = 1048576000

                            If Tamanho < GB Then
                                Tamanho /= MB
                                Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} MB"

                            Else
                                Tamanho /= GB
                                Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} GB"

                            End If

                            FormataGridView_Phonebook(False, File.Name, FileAtributes.LastWriteTime, Tamanho, Dgv)

                        End If
                    Next
                Else
                    FormataGridView_Phonebook(False, Nothing, Nothing, Nothing, Dgv)
                    Dgv.Rows.Clear()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível listar os arquivos, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Sub FileDownload_Phonebook_Cliente(Dgv As BunifuDataGridView, Fbd As FolderBrowserDialog, Cliente As String, Progress As Guna2ProgressBar, TabControl As Guna2TabControl)
        Fbd.Description = "Selecione uma pasta para realizar o Download"
        Fbd.RootFolder = Environment.SpecialFolder.MyComputer
        Fbd.ShowNewFolderButton = True

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Fbd.ShowDialog = DialogResult.OK Then
                    Dim Valida As Boolean
                    For Each row As DataGridViewRow In Dgv.Rows()
                        If Convert.ToBoolean(row.Cells(0).Value) Then
                            Dim Files As String = row.Cells(1).Value
                            If Sftp.Exists($"{My.Settings.SFTP_PhonebookClientes}{Cliente}/{Path.GetFileName(Files)}") Then
                                Using stream As Stream = File.OpenWrite($"{Fbd.SelectedPath}\{Files}")
                                    Sftp.DownloadFile($"{My.Settings.SFTP_PhonebookClientes}{Cliente}/{Path.GetFileName(Files)}", stream)
                                    Valida = True
                                End Using
                            End If
                        End If
                    Next

                    If Valida = True Then
                        MessageBox.Show($"Download realizado com sucesso.", "informação", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível fazer o download do arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Sub FileDownload_Phonebook_Colaborador(Dgv As BunifuDataGridView, Fbd As FolderBrowserDialog, Colaborador As String, Progress As Guna2ProgressBar, TabControl As Guna2TabControl)
        Fbd.Description = "Selecione uma pasta para realizar o Download"
        Fbd.RootFolder = Environment.SpecialFolder.MyComputer
        Fbd.ShowNewFolderButton = True

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Fbd.ShowDialog = DialogResult.OK Then
                    Dim Valida As Boolean
                    For Each row As DataGridViewRow In Dgv.Rows()
                        If Convert.ToBoolean(row.Cells(0).Value) Then
                            Dim Files As String = row.Cells(1).Value
                            If Sftp.Exists($"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}/{Path.GetFileName(Files)}") Then
                                Using stream As Stream = File.OpenWrite($"{Fbd.SelectedPath}\{Files}")
                                    Sftp.DownloadFile($"{My.Settings.SFTP_PhonebookColaborador}{Colaborador}/{Path.GetFileName(Files)}", stream)
                                    Valida = True
                                End Using
                            End If
                        End If
                    Next

                    If Valida = True Then
                        MessageBox.Show($"Download realizado com sucesso.", "informação", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível fazer o download do arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Sub FileDownload_Phonebook_Fornecedor(Dgv As BunifuDataGridView, Fbd As FolderBrowserDialog, Fornecedor As String, Progress As Guna2ProgressBar, TabControl As Guna2TabControl)
        Fbd.Description = "Selecione uma pasta para realizar o Download"
        Fbd.RootFolder = Environment.SpecialFolder.MyComputer
        Fbd.ShowNewFolderButton = True

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Fbd.ShowDialog = DialogResult.OK Then
                    Dim Valida As Boolean
                    For Each row As DataGridViewRow In Dgv.Rows()
                        If Convert.ToBoolean(row.Cells(0).Value) Then
                            Dim Files As String = row.Cells(1).Value
                            If Sftp.Exists($"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}/{Path.GetFileName(Files)}") Then
                                Using stream As Stream = File.OpenWrite($"{Fbd.SelectedPath}\{Files}")
                                    Sftp.DownloadFile($"{My.Settings.SFTP_PhonebookFornecedor}{Fornecedor}/{Path.GetFileName(Files)}", stream)
                                    Valida = True
                                End Using
                            End If
                        End If
                    Next

                    If Valida = True Then
                        MessageBox.Show($"Download realizado com sucesso.", "informação", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível fazer o download do arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Function FileDelete_Phonebook(Dgv As BunifuDataGridView, Panel As Guna2ShadowPanel, Path As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()
                Dim i As Integer = 0
                While i < Dgv.Rows.Count
                    Dim row As DataGridViewRow = Dgv.Rows(i)
                    If Convert.ToBoolean(row.Cells(0).Value) Then
                        Dim Files As String = row.Cells(1).Value
                        Sftp.DeleteFile($"{Path}{Files}")
                        Dgv.Rows.Remove(row)
                        Panel.Visible = False
                        i -= 1
                    End If
                    i += 1
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível excluir o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Sub PathDelete_Phonebook(Path As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()
                If Sftp.Exists($"{Path}") Then
                    Dim Files = Sftp.ListDirectory($"{Path}")
                    For Each File In Files
                        If File.IsRegularFile Then
                            Sftp.DeleteFile($"{Path}{File.Name}")
                        End If
                    Next
                    Sftp.DeleteDirectory($"{Path}")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível excluir a pasta do registro, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

#End Region




    Public Function FormataGridView_Helpdesk(ColumnChkBox As Boolean, ColumnName As String, ColumnData As String, ColumnTamanho As String, Dgv As BunifuDataGridView)
        With Dgv

            .GridColor = Color.WhiteSmoke
            .DefaultCellStyle.Font = New Font("Segoe UI Semibold", 9)
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(223, 243, 251)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.White
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .RowHeadersDefaultCellStyle.Font = New Font("Segoe UI Bold", 9)
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersHeight = 40
            .RowTemplate.Height = 40

            .ColumnCount = 5

            .Rows.Add(ColumnChkBox, ColumnName, ColumnData, ColumnTamanho)

            .Columns(1).HeaderText = "Arquivo"
            .Columns(2).HeaderText = "Data"
            .Columns(3).HeaderText = "Tamanho"
            .Columns(0).HeaderCell = New CheckHeaderCell

            .Columns(0).Visible = True
            .Columns(0).ReadOnly = False

            .Sort(.Columns(2), ListSortDirection.Descending)
            .Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        End With
        Return Dgv
    End Function

    Public Function FileList_Helpdesk(Dgv As BunifuDataGridView, Cliente As String, Finalidade As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Sftp.Exists($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}/") Then
                    Dim Files = Sftp.ListDirectory($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}/")

                    For Each File In Files
                        If File.IsRegularFile Then
                            Dim FileAtributes = Sftp.GetAttributes($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}/{File.Name}")
                            Dim Tamanho As String = FileAtributes.Size
                            Dim MB = 1048576
                            Dim GB = 1048576000

                            If Tamanho < GB Then
                                Tamanho /= MB
                                Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} MB"

                            Else
                                Tamanho /= GB
                                Tamanho = $"{FormatNumber(Tamanho, 2).Replace(",", ".")} GB"

                            End If

                            FormataGridView_Helpdesk(False, File.Name, FileAtributes.LastWriteTime, Tamanho, Dgv)

                        End If
                    Next
                Else
                    FormataGridView_Helpdesk(False, Nothing, Nothing, Nothing, Dgv)
                    Dgv.Rows.Clear()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível listar os arquivos, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Function CarregarArquivo_Helpdesk(Dgv As BunifuDataGridView, Ofd As OpenFileDialog, Cliente As String, Finalidade As String, TabControl As Guna2TabControl)
        Ofd.Title = "Selecione Arquivo"
        Ofd.Filter = "Todos os arquivos (*.*)|*.*"
        Ofd.Multiselect = True
        Ofd.CheckFileExists = True
        Ofd.CheckPathExists = True
        Ofd.FilterIndex = 2
        Ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Ofd.ReadOnlyChecked = True
        Ofd.ShowReadOnly = True
        Ofd.FileName = ""

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim FilePath() As String = Ofd.FileNames
                    Dim FileName As String

                    'VERIFICA AS INFORMAÇÕES (ULTIMA ATUALIZAÇÃO E TAMANHO) DO ARQUIVO
                    For Each FileName In FilePath
                        Dim FileWriteTime As String = File.GetLastWriteTime(FileName)
                        Dim FileSize As String = File.OpenRead(FileName).Length
                        Dim MB = 1048576
                        Dim GB = 1048576000

                        If FileSize < GB Then
                            FileSize /= MB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} MB"

                        Else
                            FileSize /= GB
                            FileSize = $"{FormatNumber(FileSize, 2).Replace(",", ".")} GB"
                        End If

                        If Not Sftp.Exists($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}") Then
                            Sftp.CreateDirectory($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}")
                        End If

                        If Not Sftp.Exists($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}") Then
                            Sftp.CreateDirectory($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}")
                        End If

                        'VERIFICA SE EXISTE O ARQUIVO NO SERVIDOR
                        If Sftp.Exists($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}/{Path.GetFileName(FileName)}") Then
                            If MessageBox.Show($"O Arquivo {Path.GetFileName(FileName)} já existe no servidor, deseja substitui-lo?", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then

                                'REMOVE A LINHA CORRESPONDENTE AO ARQUIVO QUE FOI SUBSTITUIDO
                                For Each VFile As DataGridViewRow In Dgv.Rows
                                    If VFile.Cells(1).Value.ToString.Contains(Path.GetFileName(FileName)) Then
                                        Dgv.Rows.Remove(VFile)
                                    End If
                                Next

                                'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                                Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                    Sftp.UploadFile(stream, $"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}/{Path.GetFileName(FileName)}")

                                    If Dgv.Rows.Count > 0 Then
                                        Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                    Else
                                        FormataGridView_Helpdesk(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                    End If
                                End Using
                            End If

                        Else

                            'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                            Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")

                                Sftp.UploadFile(stream, $"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}/{Path.GetFileName(FileName)}")

                                If Dgv.Rows.Count > 0 Then
                                    Dgv.Rows.Add(False, Path.GetFileName(FileName), FileWriteTime, FileSize)
                                Else
                                    FormataGridView_Helpdesk(False, Path.GetFileName(FileName), FileWriteTime, FileSize, Dgv)
                                End If
                            End Using
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível carregar o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Sub FileDownload_Helpdesk(Dgv As BunifuDataGridView, Fbd As FolderBrowserDialog, Cliente As String, Finalidade As String, Progress As Guna2ProgressBar, TabControl As Guna2TabControl)
        Fbd.Description = "Selecione uma pasta para realizar o Download"
        Fbd.RootFolder = Environment.SpecialFolder.MyComputer
        Fbd.ShowNewFolderButton = True

        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Fbd.ShowDialog = DialogResult.OK Then
                    Dim Valida As Boolean
                    For Each row As DataGridViewRow In Dgv.Rows()
                        If Convert.ToBoolean(row.Cells(0).Value) Then
                            Dim Files As String = row.Cells(1).Value
                            If Sftp.Exists($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}/{Path.GetFileName(Files)}") Then
                                Using stream As Stream = File.OpenWrite($"{Fbd.SelectedPath}\{Files}")
                                    Sftp.DownloadFile($"{My.Settings.SFTP_HelpdeskOSRemoto}{Cliente}/{Finalidade}/{Path.GetFileName(Files)}", stream)
                                    Valida = True
                                End Using
                            End If
                        End If
                    Next

                    If Valida = True Then
                        MessageBox.Show($"Download realizado com sucesso.", "INFORMAÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível fazer o download do arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Sub PathDelete_Helpdesk(Path As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()
                If Sftp.Exists($"{Path}") Then
                    If Sftp.Exists($"{Path}OCORRENCIA/") Then
                        Dim FilesOC = Sftp.ListDirectory($"{Path}OCORRENCIA/")
                        For Each File In FilesOC
                            If File.IsRegularFile Then
                                Sftp.DeleteFile($"{Path}OCORRENCIA/{File.Name}")
                            End If
                        Next
                        Sftp.DeleteDirectory($"{Path}OCORRENCIA/")
                    End If

                    If Sftp.Exists($"{Path}RESULTADO/") Then
                        Dim FilesRES = Sftp.ListDirectory($"{Path}RESULTADO/")
                        For Each File In FilesRES
                            If File.IsRegularFile Then
                                Sftp.DeleteFile($"{Path}RESULTADO/{File.Name}")
                            End If
                        Next
                        Sftp.DeleteDirectory($"{Path}RESULTADO/")
                    End If
                    Sftp.DeleteDirectory($"{Path}")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível excluir a pasta do registro, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Function FileDelete_Helpdesk(Dgv As BunifuDataGridView, Panel As Guna2ShadowPanel, Path As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()
                Dim i As Integer = 0
                While i < Dgv.Rows.Count
                    Dim row As DataGridViewRow = Dgv.Rows(i)
                    If Convert.ToBoolean(row.Cells(0).Value) Then
                        Dim Files As String = row.Cells(1).Value
                        Sftp.DeleteFile($"{Path}{Files}")
                        Dgv.Rows.Remove(row)
                        Panel.Visible = False
                        i -= 1
                    End If
                    i += 1
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível excluir o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return Dgv
    End Function

    Public Function FileList_Usuario(PbPerfil As Guna2CirclePictureBox, Usuario As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Sftp.Exists($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/") Then
                    Dim Files = Sftp.ListDirectory($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/")

                    For Each File In Files
                        If File.IsRegularFile Then
                            'Dim imageByte As Byte() = Sftp.ReadAllBytes(File.FullName)
                            'Dim mStream As MemoryStream = New MemoryStream()
                            'mStream.Write(imageByte, 0, Convert.ToInt32(imageByte.Length))
                            PbPerfil.Image = New Bitmap(Sftp.OpenRead(File.FullName), False)
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível buscar o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return PbPerfil
    End Function






    Public Function CarregarArquivo_Usuario(Ofd As OpenFileDialog, ImagePerfil As Guna2CirclePictureBox, Usuario As String)
        Ofd.Title = "Selecione a Imagem"
        Ofd.Filter = "Image File(*.JPG;*.JPEG;*.PNG;*.GIF)|*.JPG;*.JPEG;*.PNG;*.GIF"
        Ofd.Multiselect = True
        Ofd.CheckFileExists = True
        Ofd.CheckPathExists = True
        Ofd.FilterIndex = 2
        Ofd.ReadOnlyChecked = True
        Ofd.ShowReadOnly = True
        Ofd.FileName = ""
        Ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()

                If Ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim FilePath() As String = Ofd.FileNames
                    Dim FileName As String

                    'VERIFICA AS INFORMAÇÕES (ULTIMA ATUALIZAÇÃO E TAMANHO) DO ARQUIVO

                    For Each FileName In FilePath

                        'If Not Sftp.Exists($"{My.Settings.SFTP_ImagemUsuario}{Usuario}") Then
                        '    Sftp.CreateDirectory($"{My.Settings.SFTP_ImagemUsuario}{Usuario}")
                        'End If

                        Dim Files = Sftp.ListDirectory($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/")
                        For Each File In Files
                            If File.IsRegularFile Then
                                Sftp.DeleteFile($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/{File.Name}")
                            End If
                        Next

                        'FAZ COPIA DO ARQUIVO PARA O SERVIDOR
                        Using stream As Stream = File.OpenRead($"{Path.GetFullPath(FileName)}")
                            Sftp.UploadFile(stream, $"{My.Settings.SFTP_ImagemUsuario}{Usuario}/{Path.GetFileName(FileName)}")
                            ImagePerfil.Image = Image.FromFile(FileName)
                        End Using
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível buscar o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
        Return ImagePerfil
    End Function


    Public Sub FileDelete_Usuario(Usuario As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()
                If Sftp.Exists($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/") Then
                    Dim Files = Sftp.ListDirectory($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/")
                    For Each File In Files
                        If File.IsRegularFile Then
                            Sftp.DeleteFile($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/{File.Name}")
                        End If
                    Next
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível excluir o arquivo, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Sub PathDelete_Usuario(Usuario As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()
                Dim FilesImage = Sftp.ListDirectory($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/")
                For Each File In FilesImage
                    If File.IsRegularFile Then
                        Sftp.DeleteFile($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/{File.Name}")
                    End If
                Next
                Sftp.DeleteDirectory($"{My.Settings.SFTP_ImagemUsuario}{Usuario}/")

                Dim FilesBackup = Sftp.ListDirectory($"{My.Settings.SFTP_BackupSistema}{Usuario}/")
                For Each File In FilesBackup
                    If File.IsRegularFile Then
                        Sftp.DeleteFile($"{My.Settings.SFTP_BackupSistema}{Usuario}/{File.Name}")
                    End If
                Next
                Sftp.DeleteDirectory($"{My.Settings.SFTP_BackupSistema}{Usuario}/")
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível excluir a pasta do usuário, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

    Public Sub Path_Usuario(Usuario As String)
        Try
            Using Sftp As SftpClient = Getconnection()
                Sftp.Connect()
                Sftp.CreateDirectory($"{My.Settings.SFTP_ImagemUsuario}{Usuario}")
            End Using
        Catch ex As Exception
            MessageBox.Show($"Não foi possível criar a pasta do usuário, o sistema será reiniciado, entre em contato com o suporte para mais informações.{vbNewLine}{vbNewLine}ERRO: {ex.Message}", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Restart()
        End Try
    End Sub

End Class
