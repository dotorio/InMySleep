create table users (
	user_id varchar(255) primary key,
    wallet_address varchar(255) unique,
    created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp on update current_timestamp
);

create table nfts (
	nft_id varchar(255) primary key,
    contract_address varchar(255),
    owner_address varchar(255),
    metadata_uri varchar(255),
    token_type ENUM('ERC-721', 'ERC-1155'),
	created_at timestamp default current_timestamp,
    updated_at timestamp default current_timestamp on update current_timestamp
);

create table metadata (
	metadata_id varchar(255) primary key,
    nft_id varchar(255),
    image_url varchar(255),
    description TEXT,
    attributes JSON,
    created_at timestamp default current_timestamp
);