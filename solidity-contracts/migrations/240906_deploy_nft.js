const KYSNFT = artifacts.require("KYSNFT");

module.exports = function (deployer, accounts) {
  const initialOwner = "0x03F930623D1eEfe45DAf2E83e67C7290C84e6951";
  deployer.deploy(KYSNFT, initialOwner);
};
