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
      components = New ComponentModel.Container()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmNetScan))
      tsBtn = New ToolStrip()
      txBtnScan = New ToolStripButton()
      tsBtnSep1 = New ToolStripSeparator()
      tsBtnHideOffline = New ToolStripButton()
      tsBtnHideHostname = New ToolStripButton()
      tsBtnHideMAC = New ToolStripButton()
      tsBtnHideVendor = New ToolStripButton()
      tsBtnSep2 = New ToolStripSeparator()
      scNetScan = New SplitContainer()
      txtBoxIPRange = New TextBox()
      lvDevices = New ListView()
      imgListButtons = New ImageList(components)
      tsBtn.SuspendLayout()
      CType(scNetScan, ComponentModel.ISupportInitialize).BeginInit()
      scNetScan.Panel1.SuspendLayout()
      scNetScan.Panel2.SuspendLayout()
      scNetScan.SuspendLayout()
      SuspendLayout()
      ' 
      ' tsBtn
      ' 
      tsBtn.Items.AddRange(New ToolStripItem() {txBtnScan, tsBtnSep1, tsBtnHideOffline, tsBtnHideHostname, tsBtnHideMAC, tsBtnHideVendor, tsBtnSep2})
      tsBtn.Location = New Point(0, 0)
      tsBtn.Name = "tsBtn"
      tsBtn.Size = New Size(1008, 25)
      tsBtn.TabIndex = 0
      ' 
      ' txBtnScan
      ' 
      txBtnScan.DisplayStyle = ToolStripItemDisplayStyle.Image
      txBtnScan.Image = CType(resources.GetObject("txBtnScan.Image"), Image)
      txBtnScan.ImageTransparentColor = Color.Magenta
      txBtnScan.Name = "txBtnScan"
      txBtnScan.Size = New Size(23, 22)
      txBtnScan.Text = "Scan"
      txBtnScan.ToolTipText = "Scan Range"
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
      tsBtnHideOffline.Text = "Hide Offline"
      tsBtnHideOffline.ToolTipText = "Hide offline IP's"
      ' 
      ' tsBtnHideHostname
      ' 
      tsBtnHideHostname.CheckOnClick = True
      tsBtnHideHostname.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnHideHostname.Image = CType(resources.GetObject("tsBtnHideHostname.Image"), Image)
      tsBtnHideHostname.ImageTransparentColor = Color.Magenta
      tsBtnHideHostname.Name = "tsBtnHideHostname"
      tsBtnHideHostname.Size = New Size(23, 22)
      tsBtnHideHostname.Text = "Hide Hostname"
      ' 
      ' tsBtnHideMAC
      ' 
      tsBtnHideMAC.CheckOnClick = True
      tsBtnHideMAC.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnHideMAC.Image = CType(resources.GetObject("tsBtnHideMAC.Image"), Image)
      tsBtnHideMAC.ImageTransparentColor = Color.Magenta
      tsBtnHideMAC.Name = "tsBtnHideMAC"
      tsBtnHideMAC.Size = New Size(23, 22)
      tsBtnHideMAC.Text = "Hide MAC"
      ' 
      ' tsBtnHideVendor
      ' 
      tsBtnHideVendor.CheckOnClick = True
      tsBtnHideVendor.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnHideVendor.Image = CType(resources.GetObject("tsBtnHideVendor.Image"), Image)
      tsBtnHideVendor.ImageTransparentColor = Color.Magenta
      tsBtnHideVendor.Name = "tsBtnHideVendor"
      tsBtnHideVendor.Size = New Size(23, 22)
      tsBtnHideVendor.Text = "Hide Vendor"
      ' 
      ' tsBtnSep2
      ' 
      tsBtnSep2.Name = "tsBtnSep2"
      tsBtnSep2.Size = New Size(6, 25)
      ' 
      ' scNetScan
      ' 
      scNetScan.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      scNetScan.FixedPanel = FixedPanel.Panel1
      scNetScan.Location = New Point(0, 25)
      scNetScan.Name = "scNetScan"
      ' 
      ' scNetScan.Panel1
      ' 
      scNetScan.Panel1.Controls.Add(txtBoxIPRange)
      ' 
      ' scNetScan.Panel2
      ' 
      scNetScan.Panel2.Controls.Add(lvDevices)
      scNetScan.Size = New Size(1008, 508)
      scNetScan.SplitterDistance = 130
      scNetScan.TabIndex = 2
      ' 
      ' txtBoxIPRange
      ' 
      txtBoxIPRange.AcceptsReturn = True
      txtBoxIPRange.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      txtBoxIPRange.BackColor = Color.AliceBlue
      txtBoxIPRange.Location = New Point(3, 0)
      txtBoxIPRange.Multiline = True
      txtBoxIPRange.Name = "txtBoxIPRange"
      txtBoxIPRange.Size = New Size(125, 508)
      txtBoxIPRange.TabIndex = 2
      ' 
      ' lvDevices
      ' 
      lvDevices.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      lvDevices.BackColor = Color.AliceBlue
      lvDevices.Location = New Point(0, 0)
      lvDevices.Name = "lvDevices"
      lvDevices.Size = New Size(872, 508)
      lvDevices.TabIndex = 0
      lvDevices.UseCompatibleStateImageBehavior = False
      ' 
      ' imgListButtons
      ' 
      imgListButtons.ColorDepth = ColorDepth.Depth32Bit
      imgListButtons.ImageStream = CType(resources.GetObject("imgListButtons.ImageStream"), ImageListStreamer)
      imgListButtons.TransparentColor = Color.Transparent
      imgListButtons.Images.SetKeyName(0, "Play.png")
      imgListButtons.Images.SetKeyName(1, "Online.png")
      imgListButtons.Images.SetKeyName(2, "MAC.png")
      imgListButtons.Images.SetKeyName(3, "Host.png")
      imgListButtons.Images.SetKeyName(4, "Vendor.png")
      ' 
      ' frmNetScan
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      BackColor = Color.Azure
      ClientSize = New Size(1008, 561)
      Controls.Add(scNetScan)
      Controls.Add(tsBtn)
      Icon = CType(resources.GetObject("$this.Icon"), Icon)
      Name = "frmNetScan"
      StartPosition = FormStartPosition.CenterScreen
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
   Friend WithEvents tsBtnHideMAC As ToolStripButton
   Friend WithEvents tsBtnHideHostname As ToolStripButton
   Friend WithEvents imgListButtons As ImageList
   Friend WithEvents tsBtnHideVendor As ToolStripButton

End Class
