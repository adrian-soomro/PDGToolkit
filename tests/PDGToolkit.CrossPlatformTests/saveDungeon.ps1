$tmpHtmlFileName="tmp.html"
[string]$currentLocation = Get-Location
$client = New-Object System.Net.WebClient
$client.DownloadFile("http://localhost:3000",  $(Join-Path -Path $(Get-Location) -ChildPath $tmpHtmlFileName))

[string]$content = [IO.File]::ReadAllText($tmpHtmlFileName)
Write-Host "Managed to obtain image: "
$content -match 'base64,(.*)\"\/\>' 
$b64 = $Matches.1
$b64=$b64.Replace("&#x3D;", "=")
$filename = $(Join-Path -Path $(Get-Location) -ChildPath "Windows-result.png")
$bytes = [Convert]::FromBase64String($b64)
[IO.File]::WriteAllBytes($filename, $bytes)

rm "$tmpHtmlFileName"