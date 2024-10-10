#!/bin/bash

SECRET_KEY=$(openssl rand -hex 32)

echo "CLUSTER_SECRET=$SECRET_KEY" >> .env

if [ -s ".env" ]; then
	echo "생성 완료."
else
	echo "생성 실패."
fi
