# Story 6.6: Settings, Audit Log & Reports Clients

Status: ready-for-dev

## Story

As a developer,
I want to retrieve/update settings, view audit logs, and access host reports,
so that I have complete visibility into my NPM instance.

## Acceptance Criteria

1. `client.Settings.GetAsync()` and `client.Settings.UpdateAsync(request)` work
2. `client.AuditLog.ListAsync()` returns audit log entries
3. `client.Reports.GetHostsAsync()` returns host statistics
4. Unit tests cover all three clients

## Tasks / Subtasks

- [ ] Task 1: Create settings models in `Models/Settings/`
  - [ ] `SettingsResponse`, `UpdateSettingsRequest`
- [ ] Task 2: Implement SettingsClient
  - [ ] Endpoints: `/api/settings`
  - [ ] Get and Update operations only
- [ ] Task 3: Create audit log models in `Models/AuditLog/`
  - [ ] `AuditLogEntry` response model
- [ ] Task 4: Implement AuditLogClient
  - [ ] Endpoint: `/api/audit-log`
  - [ ] List operation only (read-only)
- [ ] Task 5: Create reports models in `Models/Reports/`
  - [ ] `HostReportResponse` model
- [ ] Task 6: Implement ReportsClient
  - [ ] Endpoint: `/api/reports/hosts`
  - [ ] GetHosts operation only (read-only)
- [ ] Task 7: Wire all three into NginxProxyManagerClient root
- [ ] Task 8: Write unit tests for all three clients (AC: #4)

## Dev Notes

- **Three simple clients** bundled in one story since each has minimal operations
- **Settings endpoint:** `/api/settings` — get/update default site configuration
- **Audit log endpoint:** `/api/audit-log` — read-only list
- **Reports endpoint:** `/api/reports/hosts` — read-only statistics

### References

- [Source: product-brief-distillate.md#Endpoint Map — Settings, Audit Log, Reports]
- [Source: prd.md#FR32, FR33, FR34]

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

