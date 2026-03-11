#!/bin/bash


# --------- Variables ---------
HOSTNAME="testhost"
TIMEZONE="Europe/Minsk"
USER1="user1"
USER2="user2"
GROUP1="group1"
GROUP2="group2"
IP_ADDRESS="192.168.100.80/24"
IP_GATEWAY="192.168.100.1"


# --------- Functions (virtual maschine components) ---------
set_hostname() {
    sudo hostnamectl set-hostname $HOSTNAME
}
set_timezone() {
    sudo timedatectl set-timezone $TIMEZONE
}
setup_ntp_server () {
    sudo bash -c "
	sed -i 's/#NTP=/NTP=0.pool.ntp.org 1.pool.ntp.org/' /etc/systemd/timesyncd.conf
    	sed -i 's/#FallbackNTP=ntp.ubuntu.com/FallbackNTP=ntp.ubuntu.com' /etc/systemd/timesyncd.conf
	systemctl restart systemd-timesyncd
    "
}
setup_netplan() {
    sudo bash -c "
	echo 'network:
  version: 2
  renderer: networkd
  ethernets:
    enp0s3:
      dhcp4: false
      addresses:
        - $IP_ADDRESS
      routes:
        - to: default
          via: $IP_GATEWAY
      nameservers:
        addresses:
          - 1.1.1.1
          - 8.8.8.8' \
    | tee /etc/netplan/50-cloud-init.yaml > /dev/null

    netplan try

    if [ $? -eq 0 ]; then
        netplan apply

    "
}
setup_firewall() {
    sudo bash -c "
    	ufw allow 22/tcp
    	ufw allow 80/tcp
    	ufw enable
    "
}
set_accounts() {
    sudo bash -c "
    	useradd -m -s /bin/bash -c "$USER1" $USER1
    	useradd -m -s /bin/bash -c "$USER2" $USER2

    	groupadd $GROUP1
    	groupadd $GROUP2

    	usermod -aG $GROUP1 $USER1
    	usermod -aG $GROUP2 $USER2
    "
}
setup_permissions() {
    echo "$USER1 ALL=(ALL) NOPASSWD: /bin/systemctl status *" \
    	| sudo tee "/etc/sudoers.d/$USER1" >/dev/null
    echo "%$GROUP1 ALL=(ALL) NOPASSWD: /usr/bin/apt update, /usr/bin/apt upgrade -y" \
    	| sudo tee "/etc/sudoers.d/$GROUP1" >/dev/null

    echo "$USER2 ALL=(ALL) NOPASSWD: /bin/systemctl status *" \
   	| sudo tee "/etc/sudoers.d/$USER2" >/dev/null
    echo "%$GROUP2 ALL=(ALL) NOPASSWD: /usr/bin/apt update, /usr/bin/apt upgrade -y" \
    	| sudo tee "/etc/sudoers.d/$GROUP2" >/dev/null
}
setup_personal_access() {
    sudo bash -c "
    	mkdir /opt/$GROUP1
    	chown :$GROUP1 /opt/$GROUP1
    	chmod 770 /opt/$GROUP1
    	chmod g+s /opt/$GROUP1
    	sudo -u $USER1 touch /opt/$GROUP1/sometext.txt
    	chmod 600 /opt/$GROUP1/sometext.txt

    	mkdir /opt/$GROUP2
    	chown :$GROUP2 /opt/$GROUP2
    	chmod 770 /opt/$GROUP2
    	chmod g+s /opt/$GROUP2
    	sudo -u $USER2 touch /opt/$GROUP2/sometext.txt
    	chmod 600 /opt/$GROUP2/sometext.txt
     "
}
setup_ssh() {
    sudo bash -c "
	mkdir /home/$USER1/.ssh
    	mkdir /home/$USER2/.ssh
    	touch /home/$USER1/.ssh/authorized_keys
    	touch /home/$USER2/.ssh/authorized_keys

    	chmod 700 /home/$USER1/.ssh
    	chmod 700 /home/$USER2/.ssh
    	chmod 600 /home/$USER1/.ssh/authorized_keys
    	chmod 600 /home/$USER2/.ssh/authorized_keys
    	chown -R $USER1:$USER1 /home/$USER1/.ssh
    	chown -R $USER2:$USER2 /home/$USER2/.ssh

    	sed -i 's/#PasswordAuthentication yes/PasswordAuthentication no/' /etc/ssh/sshd_config
    	sed -i 's/#PubkeyAuthentication yes/PubkeyAuthentication yes/' /etc/ssh/sshd_config

    	systemctl restart ssh
    "
}

setup_host(){
    set_hostname
    set_timezone
    setup_ntp_server
    setup_netplan
    setup_firewall
}
setup_users(){
    set_accounts
    setup_permissions
    setup_personal_access
    setup_ssh
}


# --------- Main ---------
main() {
    case "$1" in
        host)
            setup_host
            ;;
        users)
            setup_users
            ;;
        *)
            exit 1
            ;;
    esac
}
main "$@"


