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
      tsBtnScan = New ToolStripButton()
      tsBtnSep1 = New ToolStripSeparator()
      tsBtnHideOffline = New ToolStripButton()
      tsBtnHideHostname = New ToolStripButton()
      tsBtnHideMAC = New ToolStripButton()
      tsBtnHideVendor = New ToolStripButton()
      tsBtnSep2 = New ToolStripSeparator()
      scNetScan = New SplitContainer()
      grpBoxIPRange = New GroupBox()
      txtBoxIPRange = New TextBox()
      contextMnuIP = New ContextMenuStrip(components)
      contextMnuIP_rescan = New ToolStripMenuItem()
      contextMnuIP_SaveAsPreset = New ToolStripMenuItem()
      contextMnuIP_LoadPreset = New ToolStripMenuItem()
      lvDevices = New ListView()
      tsBtnPause = New ToolStripButton()
      tsBtnStop = New ToolStripButton()
      tsBtn.SuspendLayout()
      CType(scNetScan, ComponentModel.ISupportInitialize).BeginInit()
      scNetScan.Panel1.SuspendLayout()
      scNetScan.Panel2.SuspendLayout()
      scNetScan.SuspendLayout()
      grpBoxIPRange.SuspendLayout()
      contextMnuIP.SuspendLayout()
      SuspendLayout()
      ' 
      ' tsBtn
      ' 
      tsBtn.AutoSize = False
      tsBtn.ImageScalingSize = New Size(32, 32)
      tsBtn.Items.AddRange(New ToolStripItem() {tsBtnScan, tsBtnPause, tsBtnStop, tsBtnSep1, tsBtnHideOffline, tsBtnHideHostname, tsBtnHideMAC, tsBtnHideVendor, tsBtnSep2})
      tsBtn.Location = New Point(0, 0)
      tsBtn.Name = "tsBtn"
      tsBtn.Size = New Size(1008, 40)
      tsBtn.TabIndex = 0
      ' 
      ' tsBtnScan
      ' 
      tsBtnScan.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnScan.Image = CType(resources.GetObject("tsBtnScan.Image"), Image)
      tsBtnScan.ImageTransparentColor = Color.Magenta
      tsBtnScan.Name = "tsBtnScan"
      tsBtnScan.Size = New Size(36, 37)
      tsBtnScan.Text = "Scan"
      tsBtnScan.ToolTipText = "Scan Range"
      '
      ' tsBtnSep1
      ' 
      tsBtnSep1.Name = "tsBtnSep1"
      tsBtnSep1.Size = New Size(6, 40)
      ' 
      ' tsBtnHideOffline
      ' 
      tsBtnHideOffline.CheckOnClick = True
      tsBtnHideOffline.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnHideOffline.Image = CType(resources.GetObject("tsBtnHideOffline.Image"), Image)
      tsBtnHideOffline.ImageTransparentColor = Color.Magenta
      tsBtnHideOffline.Name = "tsBtnHideOffline"
      tsBtnHideOffline.Size = New Size(36, 37)
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
      tsBtnHideHostname.Size = New Size(36, 37)
      tsBtnHideHostname.Text = "Hide Hostname"
      ' 
      ' tsBtnHideMAC
      ' 
      tsBtnHideMAC.CheckOnClick = True
      tsBtnHideMAC.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnHideMAC.Image = CType(resources.GetObject("tsBtnHideMAC.Image"), Image)
      tsBtnHideMAC.ImageTransparentColor = Color.Magenta
      tsBtnHideMAC.Name = "tsBtnHideMAC"
      tsBtnHideMAC.Size = New Size(36, 37)
      tsBtnHideMAC.Text = "Hide MAC"
      ' 
      ' tsBtnHideVendor
      ' 
      tsBtnHideVendor.CheckOnClick = True
      tsBtnHideVendor.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnHideVendor.Image = CType(resources.GetObject("tsBtnHideVendor.Image"), Image)
      tsBtnHideVendor.ImageTransparentColor = Color.Magenta
      tsBtnHideVendor.Name = "tsBtnHideVendor"
      tsBtnHideVendor.Size = New Size(36, 37)
      tsBtnHideVendor.Text = "Hide Vendor"
      ' 
      ' tsBtnSep2
      ' 
      tsBtnSep2.Name = "tsBtnSep2"
      tsBtnSep2.Size = New Size(6, 40)
      ' 
      ' scNetScan
      ' 
      scNetScan.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      scNetScan.FixedPanel = FixedPanel.Panel1
      scNetScan.Location = New Point(0, 43)
      scNetScan.Name = "scNetScan"
      ' 
      ' scNetScan.Panel1
      ' 
      scNetScan.Panel1.Controls.Add(grpBoxIPRange)
      ' 
      ' scNetScan.Panel2
      ' 
      scNetScan.Panel2.Controls.Add(lvDevices)
      scNetScan.Size = New Size(1008, 490)
      scNetScan.SplitterDistance = 130
      scNetScan.TabIndex = 2
      ' 
      ' grpBoxIPRange
      ' 
      grpBoxIPRange.Controls.Add(txtBoxIPRange)
      grpBoxIPRange.Dock = DockStyle.Fill
      grpBoxIPRange.Location = New Point(0, 0)
      grpBoxIPRange.Name = "grpBoxIPRange"
      grpBoxIPRange.Size = New Size(130, 490)
      grpBoxIPRange.TabIndex = 3
      grpBoxIPRange.TabStop = False
      grpBoxIPRange.Text = "IP Range"
      ' 
      ' txtBoxIPRange
      ' 
      txtBoxIPRange.AcceptsReturn = True
      txtBoxIPRange.BackColor = Color.AliceBlue
      txtBoxIPRange.ContextMenuStrip = contextMnuIP
      txtBoxIPRange.Dock = DockStyle.Fill
      txtBoxIPRange.Location = New Point(3, 19)
      txtBoxIPRange.Multiline = True
      txtBoxIPRange.Name = "txtBoxIPRange"
      txtBoxIPRange.Size = New Size(124, 468)
      txtBoxIPRange.TabIndex = 3
      ' 
      ' contextMnuIP
      ' 
      contextMnuIP.Items.AddRange(New ToolStripItem() {contextMnuIP_rescan, contextMnuIP_SaveAsPreset, contextMnuIP_LoadPreset})
      contextMnuIP.Name = "contextMnuIP"
      contextMnuIP.Size = New Size(148, 70)
      ' 
      ' contextMnuIP_rescan
      ' 
      contextMnuIP_rescan.Name = "contextMnuIP_rescan"
      contextMnuIP_rescan.Size = New Size(147, 22)
      contextMnuIP_rescan.Text = "Rescan"
      ' 
      ' contextMnuIP_SaveAsPreset
      ' 
      contextMnuIP_SaveAsPreset.Name = "contextMnuIP_SaveAsPreset"
      contextMnuIP_SaveAsPreset.Size = New Size(147, 22)
      contextMnuIP_SaveAsPreset.Text = "Save as preset"
      ' 
      ' contextMnuIP_LoadPreset
      ' 
      contextMnuIP_LoadPreset.Name = "contextMnuIP_LoadPreset"
      contextMnuIP_LoadPreset.Size = New Size(147, 22)
      contextMnuIP_LoadPreset.Text = "Load preset"
      ' 
      ' lvDevices
      ' 
      lvDevices.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      lvDevices.BackColor = Color.AliceBlue
      lvDevices.Location = New Point(0, 3)
      lvDevices.Name = "lvDevices"
      lvDevices.Size = New Size(872, 487)
      lvDevices.TabIndex = 0
      lvDevices.UseCompatibleStateImageBehavior = False
      ' 
      ' tsBtnPause
      ' 
      tsBtnPause.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnPause.Image = CType(resources.GetObject("tsBtnPause.Image"), Image)
      tsBtnPause.ImageTransparentColor = Color.Magenta
      tsBtnPause.Name = "tsBtnPause"
      tsBtnPause.Size = New Size(36, 37)
      tsBtnPause.Text = "Pause"
      tsBtnPause.ToolTipText = "Pause / Resume scan"
      '
      ' tsBtnStop
      ' 
      tsBtnStop.DisplayStyle = ToolStripItemDisplayStyle.Image
      tsBtnStop.Image = CType(resources.GetObject("tsBtnStop.Image"), Image)
      tsBtnStop.ImageTransparentColor = Color.Magenta
      tsBtnStop.Name = "tsBtnStop"
      tsBtnStop.Size = New Size(36, 37)
      tsBtnStop.Text = "Stop"
      tsBtnStop.ToolTipText = "Stop scan"
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
      scNetScan.Panel2.ResumeLayout(False)
      CType(scNetScan, ComponentModel.ISupportInitialize).EndInit()
      scNetScan.ResumeLayout(False)
      grpBoxIPRange.ResumeLayout(False)
      grpBoxIPRange.PerformLayout()
      contextMnuIP.ResumeLayout(False)
      ResumeLayout(False)
   End Sub

   Friend WithEvents tsBtn As ToolStrip
   Friend WithEvents scNetScan As SplitContainer
   Friend WithEvents lvDevices As ListView
   Friend WithEvents tsBtnScan As ToolStripButton
   Friend WithEvents tsBtnHideOffline As ToolStripButton
   Friend WithEvents tsBtnSep1 As ToolStripSeparator
   Friend WithEvents tsBtnSep2 As ToolStripSeparator
   Friend WithEvents tsBtnHideMAC As ToolStripButton
   Friend WithEvents tsBtnHideHostname As ToolStripButton
   Friend WithEvents tsBtnHideVendor As ToolStripButton
   Friend WithEvents contextMnuIP As ContextMenuStrip
   Friend WithEvents contextMnuIP_rescan As ToolStripMenuItem
   Friend WithEvents contextMnuIP_SaveAsPreset As ToolStripMenuItem
   Friend WithEvents contextMnuIP_LoadPreset As ToolStripMenuItem
   Friend WithEvents grpBoxIPRange As GroupBox
   Friend WithEvents txtBoxIPRange As TextBox
   Friend WithEvents tsBtnPause As ToolStripButton
   Friend WithEvents tsBtnStop As ToolStripButton

End Class
