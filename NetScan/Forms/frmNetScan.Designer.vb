<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNetScan
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Dispose(disposing As Boolean)
      Try
         If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()>
   Private Sub InitializeComponent()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNetScan))
      tsBtn = New ToolStrip()
      txBtnScan = New ToolStripButton()
      tsBtnSep1 = New ToolStripSeparator()
      tsBtnHideOffline = New ToolStripButton()
      tsBtnSep2 = New ToolStripSeparator()
      scNetScan = New SplitContainer()
      txtBoxIPRange = New TextBox()
      lvDevices = New ListView()
      tsBtn.SuspendLayout()
      CType(scNetScan, ComponentModel.ISupportInitialize).BeginInit()
      scNetScan.Panel1.SuspendLayout()
      scNetScan.Panel2.SuspendLayout()
      scNetScan.SuspendLayout()
      SuspendLayout()
      ' 
      ' tsBtn
      ' 
      tsBtn.Items.AddRange(New ToolStripItem() {txBtnScan, tsBtnSep1, tsBtnHideOffline, tsBtnSep2})
      tsBtn.Location = New Point(0, 0)
      tsBtn.Name = "tsBtn"
      tsBtn.Size = New Size(884, 25)
      tsBtn.TabIndex = 0
      tsBtn.Text = "ToolStrip1"
      ' 
      ' txBtnScan
      ' 
      txBtnScan.DisplayStyle = ToolStripItemDisplayStyle.Image
      txBtnScan.Image = CType(resources.GetObject("txBtnScan.Image"), Image)
      txBtnScan.ImageTransparentColor = Color.Magenta
      txBtnScan.Name = "txBtnScan"
      txBtnScan.Size = New Size(23, 22)
      txBtnScan.Text = "ToolStripButton1"
      ' 
      ' tsBtnSep1
      ' 
      tsBtnSep1.Name = "tsBtnSep1"
      tsBtnSep1.Size = New Size(6, 25)
      ' 
      ' tsBtnHideOffline
      ' 
      tsBtnHideOffline.CheckOnClick = True
      tsBtnHideOffline.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnHideOffline.Image = CType(resources.GetObject("tsBtnHideOffline.Image"), Image)
      tsBtnHideOffline.ImageTransparentColor = Color.Magenta
      tsBtnHideOffline.Name = "tsBtnHideOffline"
      tsBtnHideOffline.Size = New Size(23, 22)
      tsBtnHideOffline.Text = "ToolStripButton1"
      tsBtnHideOffline.ToolTipText = "Hide offline IP's"
      ' 
      ' tsBtnSep2
      ' 
      tsBtnSep2.Name = "tsBtnSep2"
      tsBtnSep2.Size = New Size(6, 25)
      ' 
      ' scNetScan
      ' 
      scNetScan.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      scNetScan.Location = New Point(0, 28)
      scNetScan.Name = "scNetScan"
      ' 
      ' scNetScan.Panel1
      ' 
      scNetScan.Panel1.Controls.Add(txtBoxIPRange)
      ' 
      ' scNetScan.Panel2
      ' 
      scNetScan.Panel2.Controls.Add(lvDevices)
      scNetScan.Size = New Size(884, 402)
      scNetScan.SplitterDistance = 165
      scNetScan.TabIndex = 2
      ' 
      ' txtBoxIPRange
      ' 
      txtBoxIPRange.Dock = DockStyle.Fill
      txtBoxIPRange.Location = New Point(0, 0)
      txtBoxIPRange.Multiline = True
      txtBoxIPRange.Name = "txtBoxIPRange"
      txtBoxIPRange.Size = New Size(165, 402)
      txtBoxIPRange.TabIndex = 2
      ' 
      ' lvDevices
      ' 
      lvDevices.Dock = DockStyle.Fill
      lvDevices.Location = New Point(0, 0)
      lvDevices.Name = "lvDevices"
      lvDevices.Size = New Size(715, 402)
      lvDevices.TabIndex = 0
      lvDevices.UseCompatibleStateImageBehavior = False
      ' 
      ' frmNetScan
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(884, 461)
      Controls.Add(scNetScan)
      Controls.Add(tsBtn)
      Name = "frmNetScan"
      Text = "NetScan"
      tsBtn.ResumeLayout(False)
      tsBtn.PerformLayout()
      scNetScan.Panel1.ResumeLayout(False)
      scNetScan.Panel1.PerformLayout()
      scNetScan.Panel2.ResumeLayout(False)
      CType(scNetScan, ComponentModel.ISupportInitialize).EndInit()
      scNetScan.ResumeLayout(False)
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents tsBtn As ToolStrip
   Friend WithEvents scNetScan As SplitContainer
   Friend WithEvents txtBoxIPRange As TextBox
   Friend WithEvents lvDevices As ListView
   Friend WithEvents txBtnScan As ToolStripButton
   Friend WithEvents tsBtnHideOffline As ToolStripButton
   Friend WithEvents tsBtnSep1 As ToolStripSeparator
   Friend WithEvents tsBtnSep2 As ToolStripSeparator

End Class
