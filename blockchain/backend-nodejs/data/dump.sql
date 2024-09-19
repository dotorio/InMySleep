INSERT INTO users (user_id, password, wallet_address)
VALUES 
('user_1', '1', '0xFbE4946eB5d9f194C256A45a8BAc8faDa5Daa338'),
('user_2', '1', '0x0F00caaCb2AEBB3A7bf44543192CC1F2919774b7'),
('user_3', '1', '0xfc48396440e49bF9BbbcfBc1Bf402e9B15c06F75');

INSERT INTO metadata (metadata_uri, image_url, description, attributes)
VALUES
('ipfs://QmYuvDM8SBfX6nUXDuKtFhkw69jU3qTrW66RkMRNwUr3fv', 'ipfs://QmQN33ZfowrA9VFTLTUgyGnLNYQwmRxnYBWdiTy7MYPprW', 'This is NFT #1', '{"trait_type": "Background", "value": "Red"}'),
('ipfs://QmRFJkBhS8yiQbJakUr2cpyTACiE5V57UEArBqM95GVSh3', 'ipfs://QmZHnCCsJAPAJXQ2Ef8tugZosGkXUdZutZ248JzTCmeU9h', 'This is NFT #2', '{"trait_type": "Eyes", "value": "Laser"}'),
('ipfs://QmTbrJNKcQaYGm7bN6fnJkGjWS175ik8ngXRf4FwUTj5Rv', 'ipfs://QmUwcxXCWZ4BHn8SeQyjdkaqmoNtDzSm28T8XB26ye7TQQ', 'This is NFT #3', '{"trait_type": "Fur", "value": "Golden"}'),
('ipfs://QmdcsjSdUSzKED5BrrzDDXhKxJcxhHrqh2pJL63SMZQrb4', 'ipfs://Qmb4Q1kkw2GmQh6bCBaX2Z8d23AvLuWorQLN7daQsHpPHN', 'This is NFT #4', '{"trait_type": "4", "value": "4"}'),
('ipfs://QmSi8NNDeLAbTVVnmsm8qh1pumtF3wQzgDjzCm9NuZ482n', 'ipfs://QmQLv1mHxMTrgw9tZnCKLZNdpdP1yZYPYtaGgteJy9npFH', 'This is NFT #5', '{"trait_type": "5", "value": "5"}'),
('ipfs://QmWmtZfCM4KdL9CN6SZNyEYxF4macLqPnjSzY3T3s5qhFi', 'ipfs://Qmbj9Au5hz7kZTDwwyDmKtHrHoXZVLEnmwJtMYqDkmAWD8', 'This is NFT #6', '{"trait_type": "6", "value": "6"}');

-- INSERT INTO nfts (contract_address, owner_address, metadata_id, token_type)
-- VALUES
-- ('0xcontract1', '0xabc123456789abcdef123456789abcdef1234567', 1, 'ERC-721'),
-- ('0xcontract2', '0xdef123456789abcdef123456789abcdef1234567', 2, 'ERC-1155'),
-- ('0xcontract3', '0xghi123456789abcdef123456789abcdef1234567', 3, 'ERC-721');