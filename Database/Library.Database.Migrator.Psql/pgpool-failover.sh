# Execute command by failover.
# special values:  %d = node id
#                  %h = host name
#                  %p = port number
#                  %D = database cluster path
#                  %m = new master node id
#                  %M = old master node id
#                  %H = new master node host name
#                  %P = old primary node id
#                  %% = '%' character
failed_node_id=$1
failed_host_name=$2
failed_port=$3
failed_db_cluster=$4
new_master_id=$5
old_master_id=$6
new_master_host_name=$7
old_primary_node_id=$8

 
if [ $failed_node_id = $old_master_id ];then
    touch /var/failover-log && echo "Failover occurred. Failed node: $failed_host_name. New primary: $new_master_host_name." >> /var/failover-log
    ssh root@$new_master_host_name 'su - postgres -c "repmgr -f /etc/repmgr.conf standby promote"'
fi

exit 0