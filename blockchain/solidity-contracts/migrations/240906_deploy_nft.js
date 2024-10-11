const KYSNFT = artifacts.require("KYSNFT");

module.exports = function (deployer, network, accounts) {
  deployer.deploy(KYSNFT, accounts[0]);
};
