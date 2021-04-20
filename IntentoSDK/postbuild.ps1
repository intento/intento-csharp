$file=$args[0]
Write-Host $file
$res = signtool.exe verify /pa /q $file
if (!$?)
{
	Write-Host signing $file 
	signtool.exe sign /q /d "IntentoSDK Library" /du "https://inten.to" /fd SHA256 /tr http://ts.ssl.com /td sha256 /sha1 d79d7faf87aa9eecc1437e7da38e81f8a547dc38 $file
}
else
{
	Write-Host Alerady signed $file
}



