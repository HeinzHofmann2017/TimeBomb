git pull --rebase
dotnet publish -c Release -o /var/www/TimeBomb/
sudo chown -r /var/www/TimeBomb
sudo systemctl restart TimeBomb.service
sudo systemctl status TimeBomb.service