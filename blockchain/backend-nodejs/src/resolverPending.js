const { Web3 } = require("web3");
const web3 = new Web3("https://rpc-amoy.polygon.technology"); // RPC URL

async function cancelTransaction() {
  const account = "0x90a5DD66824D81bc5A0d41A31F188e35C6D27F3c";
  const privateKey =
    "86766e852754444586985256d5c6526f58c8898c8b59840b1f0707da0fb495ea";

  const tx = {
    from: account,
    to: account, // 0 MATIC을 전송
    value: "0x0", // 0 POL
    gas: 6000000,
    gasPrice: web3.utils.toWei("30", "gwei"), // 높은 가스 가격으로 설정
    nonce: 0, // 취소하려는 트랜잭션의 nonce 값
  };

  const signedTx = await web3.eth.accounts.signTransaction(tx, privateKey);
  web3.eth
    .sendSignedTransaction(signedTx.rawTransaction)
    .on("receipt", console.log)
    .on("error", console.error);
}

cancelTransaction();
