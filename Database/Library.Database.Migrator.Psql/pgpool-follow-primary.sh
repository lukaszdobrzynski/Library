#!/bin/bash

# Special values:
# 1)  %d = node id
# 2)  %h = hostname
# 3)  %p = port number
# 4)  %D = node database cluster path
# 5)  %m = new primary node id
# 6)  %H = new primary node hostname
# 7)  %M = old main node id
# 8)  %P = old primary node id
# 9)  %r = new primary port number
# 10) %R = new primary database cluster path
# 11) %N = old primary node hostname
# 12) %S = old primary node port number
# 13) %% = '%' character

node_id=$1
node_host=$2
node_port=$3
node_pgdata=$4
new_primary_node_id=$5
new_primary_node_host=$6
old_main_node_id=$7
old_primary_node_id=$8
new_primary_node_port=$9
new_primary_node_pgdata=$10

touch /var/follow-primary-log && echo "Current node: $node_host will be made to follow the primary node: $new_primary_node_host." >> /var/follow-primary-log
echo "$(date): follow_primary_command called with parameters: $@" >> /var/log/pgpool_follow_primary.log

if [ $node_id -ne $new_primary_node_id ];then
    touch /var/follow-primary-log && echo "Current node: $node_host will be made to follow the primary node: $new_primary_node_host." >> /var/follow-primary-log
fi