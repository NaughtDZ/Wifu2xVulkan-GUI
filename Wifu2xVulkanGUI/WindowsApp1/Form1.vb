Imports System.IO
Public Class Form1
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim check As String, isrepeat As Boolean '判断重复的未知数
        If ListBox1.SelectedIndex >= 0 Then
            For Each check In ListBox2.Items '历遍list2里看有没有和选择的文件重复
                If ListBox1.SelectedItem = check Then isrepeat = True
            Next
            If isrepeat = False Then
                ListBox2.Items.Add(ListBox1.SelectedItem)
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Text = ""
        Label5.Text = "降噪幅度" & TrackBar1.Value
        Label7.Text = TrackBar2.Value
        TextBox1.Text = Application.StartupPath & "\waifu2x-ncnn-vulkan.exe"
        TextBox3.Text = Application.StartupPath & "\models-cunet"
        TextBox9.Text = Application.StartupPath
        Getnode()
    End Sub
    Private Sub Getnode()
        Dim drv() As System.IO.DriveInfo = System.IO.DriveInfo.GetDrives() '获取盘符
        Dim drvnu As Int16 '盘符历遍序号
        Dim nodename As String 'treeview的节点
        For drvnu = 0 To drv.Length - 1
            If drv(drvnu).IsReady = True Then '判读该分区是否可用，是则添加盘符到treeview
                nodename = drv(drvnu).Name.ToString
                TreeView1.Nodes.Add(nodename)
            End If
        Next
        Dim nodenum As Int16 = TreeView1.Nodes.Count '读取添加了几个盘
        Dim nodenow As Int16 '当前要读取盘内根目录的分区
        Dim dir() As String : Dim dirnum As Int64 '其下目录名与数量
        Dim nowitem As String '当前检索目录的分区
        Dim dirinfo As DirectoryInfo '定义文件夹属性
        For nodenow = 0 To nodenum - 1
            nowitem = TreeView1.Nodes.Item(nodenow).Text
            dir = Directory.GetDirectories(nowitem) '获取盘符
            For dirnum = 0 To dir.Length - 1
                dirinfo = New DirectoryInfo(dir(dirnum)) '创建一个文件夹属性信息
                If (dirinfo.Attributes And IO.FileAttributes.Hidden) <> IO.FileAttributes.Hidden Then TreeView1.Nodes.Item(nodenow).Nodes.Add(dir(dirnum)) '判断该文件夹是否隐藏（防止不可访问）
            Next
        Next
    End Sub
    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        ListBox1.Items.Clear() '每次选定新目录清空list
        Dim filename() As String, dirpath As String, filenum As Integer, filenameNopath As String
        dirpath = e.Node.Text '获取路径
        filename = Directory.GetFiles(dirpath) '获取带路径文件名
        For filenum = 0 To filename.Length - 1
            If Strings.Right(filename(filenum), 4) = ".jpg" Or Strings.Right(filename(filenum), 4) = ".png" Then
                filenameNopath = filename(filenum)
                ListBox1.Items.Add(filenameNopath)
            End If
        Next
    End Sub

    Private Sub TreeView1_AfterExpand(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterExpand
        Dim direxp As String, dirfind As Integer, dirname As String, dirfn() As String, dirinx As Integer, dirinfo As DirectoryInfo
        direxp = e.Node.Text
        For dirfind = 0 To e.Node.Nodes.Count - 1
            dirname = e.Node.Nodes.Item(dirfind).Text
            dirfn = Directory.GetDirectories(dirname)
            For dirinx = 0 To dirfn.Length - 1
                dirinfo = New DirectoryInfo(dirfn(dirinx))
                If (dirinfo.Attributes And IO.FileAttributes.Hidden) <> IO.FileAttributes.Hidden Then e.Node.Nodes.Item(dirfind).Nodes.Add(dirfn(dirinx))
            Next
        Next '以上为添加子目录节点
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        If ListBox2.SelectedIndex >= 0 Then
            PictureBox1.ImageLocation = ListBox2.SelectedItem
            Dim imgPhoto As System.Drawing.Image = System.Drawing.Image.FromFile(ListBox2.SelectedItem) '获取图片大小
            Dim imgWidth As Integer = imgPhoto.Width
            Dim imgHeight As Integer = imgPhoto.Height
            Label3.Text = imgWidth & "X" & imgHeight
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim readytodelet As Integer
        If ListBox2.Items.Count > 0 Then
            readytodelet = ListBox2.SelectedIndex
            ListBox2.Items.Remove(ListBox2.SelectedItem)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "|*.exe" '选择主程序
        OpenFileDialog1.InitialDirectory = Application.StartupPath
        OpenFileDialog1.ShowDialog()
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Label5.Text = "降噪幅度" & TrackBar1.Value
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        MsgBox(“降噪幅度，越高降噪越强烈，至于那个-1......”)
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click
        MsgBox("1为不放大， 2为放大两倍， 更高倍数请手动操作或者找waifvulkan作者理论")
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        Label7.Text = TrackBar2.Value
    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        MsgBox("处理的区块大小，显存小的可以调小，防止崩溃,32-400")
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click
        MsgBox("不同模型，对结果有一定影响，根据图片风格选取")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        FolderBrowserDialog1.SelectedPath = Application.StartupPath '选择模型
        FolderBrowserDialog1.ShowDialog()
        TextBox3.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click
        MsgBox("如果你是笔记本或者多显卡用户的话， 可能需要手动调整， 一般笔记本独显编号为1")
    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click
        MsgBox("小图4, 4, 4, 大图2, 2, 2, 一般来说不用改")
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim sle As String
        If ListBox1.SelectedIndex >= 0 Then
            sle = ListBox1.SelectedItem
            PictureBox1.ImageLocation = sle
            Dim bmp As New Bitmap(sle)
            Label3.Text = bmp.Width.ToString() & "X" & bmp.Height.ToString()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then Beep() : MsgBox("What The Fuck Is That?")
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        FolderBrowserDialog1.SelectedPath = Application.StartupPath
        FolderBrowserDialog1.ShowDialog()
        TextBox9.Text = FolderBrowserDialog1.SelectedPath
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim round As Integer, taa As String, prg As String, imp As String, op As String, opt As String, run As String
        If CheckBox1.Checked = True Then
            taa = " -x"
        Else
            taa = ""
        End If
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = ListBox2.Items.Count
        For round = 0 To ListBox2.Items.Count - 1
            'run = TextBox1.Text & " -i " & ListBox2.Items(round) & " -o " & TextBox9.Text & "\" & ListBox2.Items(round - 1) & " N_" & TrackBar1.Value & " S_" & TrackBar2.Value & ".png" & " -n " & TrackBar1.Value & " -s " & TrackBar2.Value & " -t " & TextBox2.Text & " -m " & TextBox3.Text & " -g " & TextBox4.Text & " -j " & TextBox6.Text & ":" & TextBox7.Text & ":" & TextBox8.Text & taa
            prg = TextBox1.Text
            imp = " -i " & ListBox2.Items(round)
            op = " -o " & TextBox9.Text & "\" & Strings.Right(ListBox2.Items(round), Len(ListBox2.Items(round)) - Strings.InStrRev(ListBox2.Items(round), "\"))
            opt = " -n " & TrackBar1.Value & " -s " & TrackBar2.Value & " -t " & TextBox2.Text & " -m " & TextBox3.Text & " -g " & TextBox4.Text & " -j " & TextBox6.Text & ":" & TextBox7.Text & ":" & TextBox8.Text & taa
            run = prg & imp & op & opt
            Shell(run)
            Do While File.Exists(TextBox9.Text & "\" & Strings.Right(ListBox2.Items(round), Len(ListBox2.Items(round)) - Strings.InStrRev(ListBox2.Items(round), "\"))) = False
                Application.DoEvents()
            Loop
            ProgressBar1.Value = ProgressBar1.Value + 1
        Next
        Beep()
    End Sub

    Private Sub TreeView1_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterCollapse
        TreeView1.Nodes.Clear()
        Getnode()
    End Sub

    Private Sub TextBox2_LostFocus(sender As Object, e As EventArgs) Handles TextBox2.LostFocus
        If TextBox2.Text < 32 Then
            MsgBox("必须大于32！")
            TextBox2.Text = 32
        Else
            TextBox2.Text = Fix(TextBox2.Text)
        End If
    End Sub

    Private Sub ListBox2_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox2.DragDrop

    End Sub
End Class
