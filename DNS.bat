# نام کارت شبکه‌ای که می‌خواهید DNS آن را تغییر دهید
$interfaceName = "Wi-Fi"

# آدرس‌های DNS مورد نظر
$primaryDNS = "178.22.122.100"
$secondaryDNS = "185.51.200.2"

# تنظیم DNS
Set-DnsClientServerAddress -InterfaceAlias $interfaceName -ServerAddresses $primaryDNS, $secondaryDNS

Write-Host "DNS تنظیم شد: $primaryDNS و $secondaryDNS"# نام کارت شبکه‌ای که می‌خواهید DNS آن را تغییر دهید
pause