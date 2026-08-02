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
      ToolStrip1 = New ToolStrip()
      txBtnScan = New ToolStripButton()
      tsBtnHideOffline = New ToolStripButton()
      scNetScan = New SplitContainer()
      txtBoxIPRange = New TextBox()
      lvDevices = New ListView()
      ToolStrip1.SuspendLayout()
      CType(scNetScan, ComponentModel.ISupportInitialize).BeginInit()
      scNetScan.Panel1.SuspendLayout()
      scNetScan.Panel2.SuspendLayout()
      scNetScan.SuspendLayout()
      SuspendLayout()
      ' 
      ' ToolStrip1
      ' 
      ToolStrip1.Items.AddRange(New ToolStripItem() {txBtnScan, tsBtnHideOffline})
      ToolStrip1.Location = New Point(0, 0)
      ToolStrip1.Name = "ToolStrip1"
      ToolStrip1.Size = New Size(800, 25)
      ToolStrip1.TabIndex = 0
      ToolStrip1.Text = "ToolStrip1"
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
      scNetScan.Size = New Size(800, 391)
      scNetScan.SplitterDistance = 266
      scNetScan.TabIndex = 2
      ' 
      ' txtBoxIPRange
      ' 
      txtBoxIPRange.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
      txtBoxIPRange.Location = New Point(3, 3)
      txtBoxIPRange.Multiline = True
      txtBoxIPRange.Name = "txtBoxIPRange"
      txtBoxIPRange.Size = New Size(260, 385)
      txtBoxIPRange.TabIndex = 2
      ' 
      ' lvDevices
      ' 
      lvDevices.Dock = DockStyle.Fill
      lvDevices.Location = New Point(0, 0)
      lvDevices.Name = "lvDevices"
      lvDevices.Size = New Size(530, 391)
      lvDevices.TabIndex = 0
      lvDevices.UseCompatibleStateImageBehavior = False
      ' 
      ' frmNetScan
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(scNetScan)
      Controls.Add(ToolStrip1)
      Name = "frmNetScan"
      Text = "NetScan"
      ToolStrip1.ResumeLayout(False)
      ToolStrip1.PerformLayout()
      scNetScan.Panel1.ResumeLayout(False)
      scNetScan.Panel1.PerformLayout()
      scNetScan.Panel2.ResumeLayout(False)
      CType(scNetScan, ComponentModel.ISupportInitialize).EndInit()
      scNetScan.ResumeLayout(False)
      ResumeLayout(False)
      PerformLayout()
   End Sub

   Friend WithEvents ToolStrip1 As ToolStrip
   Friend WithEvents scNetScan As SplitContainer
   Friend WithEvents txtBoxIPRange As TextBox
   Friend WithEvents lvDevices As ListView
   Friend WithEvents txBtnScan As ToolStripButton
   Friend WithEvents tsBtnHideOffline As ToolStripButton

End Class
