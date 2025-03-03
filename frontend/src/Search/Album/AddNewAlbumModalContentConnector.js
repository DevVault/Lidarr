import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import { createSelector } from 'reselect';
import { metadataProfileNames } from 'Helpers/Props';
import { addAlbum, setAddDefault } from 'Store/Actions/searchActions';
import createDimensionsSelector from 'Store/Selectors/createDimensionsSelector';
import createSystemStatusSelector from 'Store/Selectors/createSystemStatusSelector';
import selectSettings from 'Store/Selectors/selectSettings';
import AddNewAlbumModalContent from './AddNewAlbumModalContent';

function createMapStateToProps() {
  return createSelector(
    (state, { isExistingArtist }) => isExistingArtist,
    (state) => state.search,
    (state) => state.settings.metadataProfiles,
    createDimensionsSelector(),
    createSystemStatusSelector(),
    (isExistingArtist, searchState, metadataProfiles, dimensions, systemStatus) => {
      const {
        isAdding,
        addError,
        defaults
      } = searchState;

      const {
        settings,
        validationErrors,
        validationWarnings
      } = selectSettings(defaults, {}, addError);

      // For adding single albums, default to None profile
      const noneProfile = metadataProfiles.items.find((item) => item.name === metadataProfileNames.NONE);

      return {
        isAdding,
        addError,
        showMetadataProfile: true,
        isSmallScreen: dimensions.isSmallScreen,
        validationErrors,
        validationWarnings,
        noneMetadataProfileId: noneProfile.id,
        isWindows: systemStatus.isWindows,
        ...settings
      };
    }
  );
}

const mapDispatchToProps = {
  setAddDefault,
  addAlbum
};

class AddNewAlbumModalContentConnector extends Component {

  //
  // Lifecycle

  constructor(props, context) {
    super(props, context);

    this.state = {
      metadataProfileIdDefault: props.metadataProfileId.value
    };

    // select none as default
    this.onInputChange({
      name: 'metadataProfileId',
      value: props.noneMetadataProfileId
    });
  }

  componentWillUnmount() {
    // reinstate standard default
    this.props.setAddDefault({ metadataProfileId: this.state.metadataProfileIdDefault });
  }

  //
  // Listeners

  onInputChange = ({ name, value }) => {
    this.props.setAddDefault({ [name]: value });
  };

  onAddAlbumPress = () => {
    const {
      foreignAlbumId,
      rootFolderPath,
      monitor,
      monitorNewItems,
      qualityProfileId,
      metadataProfileId,
      searchForNewAlbum,
      tags
    } = this.props;

    this.props.addAlbum({
      foreignAlbumId,
      rootFolderPath: rootFolderPath.value,
      monitor: monitor.value,
      monitorNewItems: monitorNewItems.value,
      qualityProfileId: qualityProfileId.value,
      metadataProfileId: metadataProfileId.value,
      searchForNewAlbum: searchForNewAlbum.value,
      tags: tags.value
    });
  };

  //
  // Render

  render() {
    return (
      <AddNewAlbumModalContent
        {...this.props}
        onInputChange={this.onInputChange}
        onAddAlbumPress={this.onAddAlbumPress}
      />
    );
  }
}

AddNewAlbumModalContentConnector.propTypes = {
  isExistingArtist: PropTypes.bool.isRequired,
  foreignAlbumId: PropTypes.string.isRequired,
  rootFolderPath: PropTypes.object,
  monitor: PropTypes.object.isRequired,
  monitorNewItems: PropTypes.object.isRequired,
  qualityProfileId: PropTypes.object,
  metadataProfileId: PropTypes.object,
  noneMetadataProfileId: PropTypes.number.isRequired,
  searchForNewAlbum: PropTypes.object.isRequired,
  tags: PropTypes.object.isRequired,
  onModalClose: PropTypes.func.isRequired,
  setAddDefault: PropTypes.func.isRequired,
  addAlbum: PropTypes.func.isRequired
};

export default connect(createMapStateToProps, mapDispatchToProps)(AddNewAlbumModalContentConnector);
