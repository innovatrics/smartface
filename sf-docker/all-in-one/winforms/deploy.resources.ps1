& { $BinaryFormatter = New-Object -TypeName System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
 @{ 
'PictureBox1.Name' = 'PictureBox1'
'PictureBox1.BackgroundImage' = New-Object -TypeName System.Drawing.Bitmap -ArgumentList @(New-Object -TypeName  System.IO.MemoryStream -ArgumentList @(,[System.Convert]::FromBase64String('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')))
'lnk_sfstation.Name' = 'lnk_sfstation'
'lbl_sfstation.Name' = 'lbl_sfstation'
'lbl_restapi.Name' = 'lbl_restapi'
'lbl_graphql.Name' = 'lbl_graphql'
'lnk_restapi.Name' = 'lnk_restapi'
'lnk_graphql.Name' = 'lnk_graphql'
'lbl_demo.Name' = 'lbl_demo'
'$this.Name' = 'formFinal'
}
}