# EMECCA_NMS 專案管理庫

本儲存庫採用 Monorepo 結構，整合了多個核子醫學管理系統（NMS）專案分支，並透過自動化腳本與原始 SVN 開發環境保持同步。

## 📁 目錄結構

- **`DEV25/`**: Framework/IMS 核心開發分支 (核心架構與元件庫)
- **`NMS_2026/`**: NTU 專案專用分支 (針對 2026 需求之客製化與功能增強)

## 🔄 同步機制

本儲存庫為 GitHub 備份與協作中心，原始開發環境仍位於 SVN 中。
- **SVN 位置 (DEV25)**: `E:\Development\EmeccaDotNetApp\Framework\IMS\DEV25`
- **SVN 位置 (NMS2026)**: `E:\Development\EmeccaDotNetApp\NTU\NMS_2026`

### 如何同步至 GitHub
請執行各專案目錄下的 `sync_to_github.ps1` 腳本，該腳本會：
1. 自動從 SVN 目錄抓取最新異動。
2. 整合至 Git 中央控管目錄 (`EMECCA_NMS_GIT`)。
3. 推送至本儲存庫。

## 📝 Commit Message 規範

為了保持紀錄清晰，請遵循以下格式：
- `feat(sync): [描述]` - 自動同步或新增功能
- `fix(sync): [描述]` - 修復同步邏輯或錯誤
- `docs: [描述]` - 更新文件 (如 README, CHANGELOG)

---
*Last Updated: 2026-02-23*
