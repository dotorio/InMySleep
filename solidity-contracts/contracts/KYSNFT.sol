// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;

import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Enumerable.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

contract KYSNFT is ERC721Enumerable, ERC721URIStorage, Ownable {
    constructor(address initialOwner) ERC721("In My Sleep Easter Eggs", "IMS") {}

    function mint(
        address to,
        string memory _tokenURI
    ) public onlyOwner returns (uint256) {
        uint256 newTokenId = totalSupply();
        _mint(to, newTokenId);
        _setTokenURI(newTokenId, _tokenURI);

        return newTokenId;
    }

    function tokenURI(
        uint256 tokenId
    ) public view override(ERC721, ERC721URIStorage) returns (string memory) {
        return super.tokenURI(tokenId);
    }

    function tokensOfOwner(
        address owner
    ) public view returns (uint256[] memory) {
        uint256 tokenCount = balanceOf(owner);
        uint256[] memory tokenIds = new uint256[](tokenCount);

        for (uint256 i = 0; i < tokenCount; i++) {
            tokenIds[i] = tokenOfOwnerByIndex(owner, i);
        }

        return tokenIds;
    }

    function tokenURIsOfOwner(
        address owner
    ) public view returns (string[] memory) {
        uint256 tokenCount = balanceOf(owner);
        string[] memory tokenURIs = new string[](tokenCount);

        for (uint256 i = 0; i < tokenCount; i++) {
            tokenURIs[i] = tokenURI(tokenOfOwnerByIndex(owner, i));
        }

        return tokenURIs;
    }

    function supportsInterface(
        bytes4 interfaceId
    ) public view override(ERC721Enumerable, ERC721URIStorage) returns (bool) {
        return super.supportsInterface(interfaceId);
    }

    function _beforeTokenTransfer(
        address from,
        address to,
        uint256 tokenId,
        uint256 batchSize
    ) internal override(ERC721, ERC721Enumerable) {
        super._beforeTokenTransfer(from, to, tokenId, batchSize);
    }

    function _burn(
        uint256 tokenId
    ) internal override(ERC721, ERC721URIStorage) {
        super._burn(tokenId);
    }
}
